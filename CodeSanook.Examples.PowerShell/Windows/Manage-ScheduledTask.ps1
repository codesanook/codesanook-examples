$taskNames = @(
    "ShutdownTheInstance"
)

function Format-UtcTime($dateTime) {
    $dateTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ")
}

try {
    $taskNames | ForEach-Object {
        $taskName = $_
        Disable-ScheduledTask -TaskName $taskName -ErrorAction Stop | Out-Null
    }
}
catch {
    $_.Exception.Message
    $_.Exception.StackTrace
}

$result = $taskNames | ForEach-Object {
    $taskName = $_
    $task = Get-ScheduledTask -TaskName $taskName
    $info = Get-ScheduledTaskInfo -TaskName $taskName

    [PSCustomObject]@{
        "Task Name"     = "`"$taskName`""
        "State"         = $task.State
        "Next Run Time" = $info.NextRunTime
        "Last Run Time" = $info.LastRunTime 
    }
}
"Current UTC time $( Format-UtcTime([System.DateTime]::UtcNow))"

$result | `
    Sort-Object -Property "Next Run Time" -Descending | `
    Select-Object @(
    "Task Name"
    "State"
    @{ Name = "Next Run Time"; Expression = { Format-UtcTime($_."Next Run Time") } }
    @{ Name = "Last Run Time"; Expression = { Format-UtcTime($_."Last Run Time") } }
) | Format-Table
