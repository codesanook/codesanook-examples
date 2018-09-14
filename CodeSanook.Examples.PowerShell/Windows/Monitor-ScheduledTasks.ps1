# Event filter for the initial query for all "Start" events in the last 24 hours
$eventFilter = @{
    LogName   = 'Microsoft-Windows-TaskScheduler/Operational'
    Id        = 100
    StartTime = [datetime]::Now.AddMinutes(-60)
}

# propertySelector for the Correlation id (the InstanceId) and task name
[string[]]$PropertyQueries = @(
    'Event/EventData/Data[@Name="InstanceId"]'
    'Event/EventData/Data[@Name="TaskName"]'
)

$propertySelector = New-Object System.Diagnostics.Eventing.Reader.EventLogPropertySelector @(, $PropertyQueries)

# Loop through the start events
$taskInvocations = foreach ($startEvent in Get-WinEvent -FilterHashtable $eventFilter) {
    # Grab the InstanceId and Task Name from the start event
    $InstanceId, $TaskName = $startEvent.GetPropertyValues($propertySelector)

    # Create custom object with the name and start event, query end event by InstanceId
    $startTime = $startEvent.TimeCreated.ToUniversalTime()
    $endTime = $(Get-WinEvent -FilterXPath "*[System[(EventID=102)] and EventData[Data[@Name=""InstanceId""] and Data=""{$InstanceId}""]]" -LogName 'Microsoft-Windows-TaskScheduler/Operational' -ErrorAction SilentlyContinue).TimeCreated
    
    if ($null -ne $endTime) {
        $endTime = $endTime.ToUniversalTime()
        $duration = $endTime.Subtract($startTime)

    }
    else {
        $duration = $null
    }

    $result = [PSCustomObject]@{
        TaskName  = $TaskName
        StartTime = $startTime
        EndTime   = $endTime
        Duration  = $duration
    }
  
    $result
}

$taskInvocations = $TaskInvocations | Sort-Object -Property Duration -Descending
$taskInvocations