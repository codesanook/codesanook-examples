<#
.SYNOPSIS
Explanation: When you create the trigger with New-ScheduledTaskTrigger, 
the time you specify is converted to UTC time and saved as a string in the trigger's StartBoundary property. 
On my machine, 10:15am produces a $trigger. 
StartBoundary of '2017-12-19T15:15:00Z', where the 'Z' indicates UTC time. 
To specify a local time, we need to convert this date back into local time and remove the 'Z'; 
we want '2017-12-19T10:15:00'. 

The function parses the date string, converts it to local time, and formats it in the correct format.
Credit https://social.technet.microsoft.com/Forums/en-US/e1dad780-6a99-44f2-9688-041e7026854b/quotsynchronize-across-time-zonesquot-scheduled-task-option-and-newscheduledtasktrigger?forum=winserverpowershell
#>

#$timeToShutdownUtc = [System.DateTime]::UtcNow.AddMinutes(1).ToString('h:mmtt') # For testing
$timeToShutdownUtc = '6:50pm'

function Disable-SynchronizeTimeZone {
    param(
        [parameter(ValueFromPipeline)] 
        [CimInstance] $trigger 
    )
    $newTrigger = $trigger.Clone()
    $newTrigger.StartBoundary = [DateTime]::Parse($trigger.StartBoundary).ToLocalTime().ToString("s")
    $newTrigger
}

$command = {
    $logPath = 'c:/logs'
    New-Item -Path $logPath -ItemType Directory -Force # New folder if not exist

    try {

        $logFilePattern = Join-Path -Path $logpath -ChildPath 'shutdown-*'
        $logFileLastMoreThanInDays = 7 #Remove log file last more than given day
        $utcNow = [System.DateTime]::UtcNow
        $dateTimeFormat = 'yyyy-MM-ddTHH-mm-ssZ';

        Get-ChildItem -Path $logFilePattern | Where-Object {
            $isMatch = $_ -match '(?<dateTime>\d{4}-\d{2}-\d{2}T\d{2}-\d{2}-\d{2}Z)'
            if ($isMatch) {
                $logFileUtcDateTime = [System.DateTime]::parseexact($matches['dateTime'], $dateTimeFormat, $null)
                return $logFileUtcDateTime -lt $utcNow.AddDays( -1 * [System.Math]::Abs($logFileLastMoreThanInDays))
            }
            return $false
        } | Remove-Item -Force 


        $logName = 'shutdown-{0}.txt' -f $utcNow.ToString($dateTimeFormat)
        ('Instance shutdown at {0}' -f $utcNow.ToString($dateTimeFormat)) | `
            Out-File (Join-Path -Path $logPath -ChildPath $logName)

        Stop-Computer -Force
    }
    catch {
        $_.Exception | Out-File (Join-Path -Path $logPath -ChildPath 'internal-log.txt')
    }
}

$action = New-ScheduledTaskAction -Execute "PowerShell.exe" -Argument "-Command & { $command }" 

# Specify the trigger settings in UTC time, the instance need to set time to UTC
$trigger = New-ScheduledTaskTrigger -At $timeToShutdownUtc -Daily | Disable-SynchronizeTimeZone #1.50am BKK time
$user = "NT AUTHORITY\SYSTEM" # Specify the account to run the script

Register-ScheduledTask `
    -TaskName "ShutdownTheInstance" `
    -Trigger $trigger `
    -User $user `
    -Action $action `
    -RunLevel Highest `
    -Force
