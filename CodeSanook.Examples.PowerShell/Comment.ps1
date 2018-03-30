<#
.SYNOPSIS
script to say hello to

.PARAMETER Name
    name of one you want to say hello to 

.EXAMPLE
    .\Comment.ps1 -Name "Anthony"

.NOTES
    General notes
#>
param(
    [string] $Name
)

    <#
    .SYNOPSIS
        function to say hello to

    .PARAMETER Name
        name of one you want to say hello to 

    .EXAMPLE
        Say-Hello -Name "Anthony"
    #>
function Say-Hello {
    param(
        [string] $Name
    )

    Write-Host "Hello $Name"
} 

Get-Help -Name  Say-Hello
Say-Hello -Name $Name