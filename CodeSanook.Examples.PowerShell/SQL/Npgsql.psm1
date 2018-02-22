function Try-Ok {
    param (
        [Parameter(Position = 0)]
        [string] $Before,
        [Parameter(Position = 1)]
        [ScriptBlock] $Run = $(throw new-object InvalidOperationException),
        [Parameter(Position = 2)]
        [ScriptBlock] $Catch,
        [Parameter(Position = 3)]
        [ScriptBlock] $Finally
    )
    
    write-host "$($before)... " -NoNewline
    
    try {
        & $Run -ErrorAction stop
        write-host -ForegroundColor Green "OK"
    }
    catch {
        write-host -ForegroundColor Red "ERROR"
        
        if ($Catch -ne $null) {
            & $Catch $_
        }
        
        throw
    }
    finally {
        if ($Finally -ne $null) {
            & $Finally
        }
    }
}

function Load-Driver {
    param (
        [Parameter(Mandatory = $true)]
        [string] $path
    )
    
    Try-Ok "Loading driver $path" {
        [Reflection.Assembly]::LoadFrom($path) > $null
        $script:driverLoaded = $true;
    }
}

function Clear-Pools {
    Try-Ok "Clearing all pools" {
        [Npgsql.NpgsqlConnection]::ClearAllPools()
    }
}

function Open-Connection {
    param (
        [Parameter(Mandatory = $true)]
        [string] $ConnectionString,
        [switch] $ReturnObject
    )
    
    Try-Ok "Opening connection" {
        $conn = new-object Npgsql.NpgsqlConnection $ConnectionString
        $conn.Open()
        $script:implicitConnection = $conn
        
        if ($ReturnObject.isPresent) { $conn }
    }
}

function Close-Connection {
    param (
        [Npgsql.NpgsqlConnection] $Connection = $script:implicitConnection
    )
    
    Try-Ok 'Closing connection' { $Connection.Close() }
}

function Begin-Transaction {
    param (
        [Npgsql.NpgsqlConnection] $Connection
    )
    
    Try-Ok 'Begin transaction' { $Connection.BeginTransaction() }
}

function Commit-Transaction {
    param (
        [Npgsql.NpgsqlTransaction] $Transaction
    )
    
    Try-Ok 'Committing transaction' { $Transaction.Commit() }
}

function Rollback-Transaction {
    param (
        [Npgsql.NpgsqlTransaction] $Transaction
    )
    
    Try-Ok 'Rolling back transaction' { $Transaction.Rollback() }
}

function Run-UnitOfWork {
    param (
        [Alias("UoW")]
        [ScriptBlock] $UnitOfWork = $(throw new-object InvalidOperationException 'You must supply a ScriptBlock to Run-UnitOfWork!'),
        [Alias("CS")]
        [string] $ConnectionString,
        [Npgsql.NpgsqlConnection] $Connection,
        [string] $InitFunction,
        [switch] $AutoCommit,
        [switch] $ClearConnectionPools,
        [switch] $Debug
    )
    
    $oldDebugPreference = $DebugPreference
    
    if ($Debug.IsPresent) {
        $DebugPreference = 'Continue'
    }
    
    [Npgsql.NpgsqlConnection] $myConnection
    
    if ($Connection -ne $null) {
        $myConnection = $Connection
    }
    elseif ($ConnectionString -ne $null -and $ConnectionString.Length -gt 0) {
        $myConnection = Open-Connection $ConnectionString -ReturnObject
    }
    else {
        throw new-object System.InvalidOperationException 'You must supply a connection or a connection string.'
    }
    
    $myTransaction = Begin-Transaction $myConnection
    
    if (![string]::IsNullOrEmpty($InitFunction)) {
        Try-Ok "Invoking init function $InitFunction" {
            $func = Get-Content "Function:\$InitFunction"
            if ($func -is [ScriptBlock]) {
                & $func $myConnection $myTransaction
            }
        }
    }
    
    try {
        & $UnitOfWork $myConnection $myTransaction -ErrorAction stop
        
        if ($AutoCommit.isPresent) {
            Commit-Transaction $myTransaction
        }
        elseif ($myTransaction.Connection -ne $null) {
            Rollback-Transaction $myTransaction
        }
    }
    catch {
        Rollback-Transaction $myTransaction
        throw
    }
    finally {
        if ($Connection -eq $null) {
            Close-Connection $myConnection
        }
        
        $DebugPreference = $oldDebugPreference
    }
}

function Get-CommandObject {
    param (
        [Parameter(Mandatory = $true)]
        [string] $CommandText,
        [hashtable] $Parameters,
        [Npgsql.NpgsqlConnection] $Connection = $(
            if ($myConnection -ne $null) { $myConnection }
            else { throw New-Object InvalidOperationException }),
        [Npgsql.NpgsqlTransaction] $Transaction = $(
            if ($myTransaction -ne $null) { $myTransaction }
            else { throw New-Object InvalidOperationException })
    )
    
    $cmd = New-Object Npgsql.NpgsqlCommand ($CommandText, $Connection, $Transaction)

    try {
        if ($Parameters -ne $null -and $Parameters.Count -gt 0) {
            $Parameters.GetEnumerator() | foreach { $key = $_.Key; $value = $_.Value
                if ($value.GetType() -eq @().GetType() -and $value.Length -eq 2 -and $value[1] -is [System.Data.DbType]) {
                    Get-ParameterObject $key $value[0] $value[1]
                }
                else {
                    Get-ParameterObject $key $value
                }
            } | foreach { $cmd.Parameters.Add($_) > $null }
        }
    }
    catch {
        debug-Command $cmd $Parameters
        throw $_.Exception
    }
    
    $cmd
}

function Get-ParameterObject {
    param (
        [string] $Name,
        [object] $Value,
        [System.Data.DbType] $DbType
    )
    
    if ($DbType -eq $null) {
        new-object Npgsql.NpgsqlParameter ($Name, $Value)
    }
    else {
        $p = new-object Npgsql.NpgsqlParameter ($Name, $DbType)
        $p.Value = $Value
        $p
    }
}

function debug-Command {
    param ([Npgsql.NpgsqlCommand] $Command,
        [Hashtable] $Parameters)
    
    ($cmd.CommandText -replace '\s+', ' ').Trim()

    if ($Parameters -ne $null -and $Parameters.Count -gt 0) {
        "`n"

        $params = ($Parameters.GetEnumerator() | % {
                if ($_.Value -is [byte[]]) { $val = "<byte[]>" }
                else {
                    $val = switch ($_.Value) {
                        {$_ -is [string]} { "'$_'" }
                        {$_ -is [int] `
                                -or $_ -is [decimal] `
                                -or $_ -is [float] `
                                -or $_ -is [double] `
                                -or $_ -is [long]
                        } { $_ }
                        default { "<$($_.GetType())>" }
                    }
                }
            
                "$($_.Key): $val"
            }) -join "`n`t"
        
        "Parameters: `n`t$params"
    }
}

function Invoke-Query {
    param (
        [Parameter(Mandatory = $true)]
        [string] $Query,

        [hashtable] $Parameters,
        [Npgsql.NpgsqlConnection] $Connection = $(
            if ($myConnection -ne $null) { $myConnection }
            else { throw New-Object InvalidOperationException }),
        [Npgsql.NpgsqlTransaction] $Transaction = $(
            if ($myTransaction -ne $null) { $myTransaction }
            else { throw New-Object InvalidOperationException }),
        [switch] $Void,
        [switch] $Scalar,
        [string] $ReturnId
    )
    
    $cmd = Get-CommandObject -Connection $Connection `
        -Transaction $Transaction `
        -CommandText $Query `
        -Parameters $Parameters
    
    if ($Debug) {
        write-debug "$(debug-Command $cmd $Parameters)"
    }
    
    try {
        if ($Void.isPresent) {
            $cmd.ExecuteNonQuery() > $null
            if (![string]::IsNullOrEmpty($ReturnId)) {
                Get-CurrVal $ReturnId $Connection $Transaction
            }
        }
        elseif ($Scalar.isPresent) {
            $cmd.ExecuteScalar()
        }
        else {
            $dr = $cmd.ExecuteReader()
            $out = @()
            $fc = $dr.FieldCount
            
            $fields = 0..($fc - 1) | foreach { $dr.GetName($_) }
            
            while ($dr.Read()) {
                $row = @{}
                $fields | foreach {
                    $row[$_] = $dr[$_]
                }
                $out += $row
            }

            $dr.Close() #fix System.InvalidOperationException: An operation is already in progress.
            $out
        }
    }
    catch {
        Write-Host "Could not invoke command:"
        Write-Host (debug-Command $cmd $Parameters)
        throw
    }
}

function Get-CurrVal {
    param (
        [Parameter(Mandatory = $true)]
        [string] $SequenceName,
        [Npgsql.NpgsqlConnection] $Connection = $(
            if ($myConnection -ne $null) { $myConnection }
            else { throw New-Object InvalidOperationException }),
        [Npgsql.NpgsqlTransaction] $Transaction = $(
            if ($myTransaction -ne $null) { $myTransaction }
            else { throw New-Object InvalidOperationException })
    )
    
    Invoke-Query -Connection $Connection `
        -Transaction $Transaction `
        -Query "select currval('$SequenceName')" `
        -Scalar
}

Export-ModuleMember Load-Driver
Export-ModuleMember Clear-Pools
Export-ModuleMember Open-Connection
Export-ModuleMember Close-Connection
Export-ModuleMember Begin-Transaction
Export-ModuleMember Commit-Transaction
Export-ModuleMember Rollback-Transaction
Export-ModuleMember Run-UnitOfWork
Export-ModuleMember Invoke-Query
Export-ModuleMember Get-CurrVal
Export-ModuleMember Get-CommandObject
Export-ModuleMember Get-ParameterObject
Export-ModuleMember Get-Data