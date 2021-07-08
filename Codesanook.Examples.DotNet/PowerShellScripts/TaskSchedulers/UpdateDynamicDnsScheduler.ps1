<#
    Usage:
    Update freeDnsDomainId and run the following command with administrator:
   ./UpdateDynamicDnsScheduler.ps1

    Note, it works only for Windows OS or Windows Server.
#>

# We need to use a script block to passs as argument of New-ScheduledTaskAction and use single quote
$command = {
    try {
        # Change these variables to match your environment
        $freeDnsDomainId = ''
        $logDirectory = 'c:/logs'

        New-Item -Path $logDirectory -ItemType Directory -Force # Create a new folder if not exist
        $logFilePrefix = 'update-dynamic-dns'
        $logFilePattern = Join-Path -Path $logDirectory -ChildPath ('{0}-*' -f $logFilePrefix)
        $logFileLastMoreThanInDays = 7 # Remove a log file that lasts more than given days
        $utcNow = [System.DateTime]::UtcNow

        # We can't use : in a file name.
        $dateFormat = 'yyyy-MM-dd';

        Get-ChildItem -Path $logFilePattern | Where-Object {
            # Math only date portion in a log file name
            $isMatch = $_ -match '(?<date>\d{4}-\d{2}-\d{2})'
            if ($isMatch) {
                $logFileUtcDateTime = [System.DateTime]::ParseExact($matches['date'], $dateFormat, [System.Globalization.CultureInfo]::InvariantCulture)
                # We need return here for an early exit.
                return $logFileUtcDateTime -lt $utcNow.AddDays(-1 * [System.Math]::Abs($logFileLastMoreThanInDays))
            }
            $false
        } | Remove-Item -Force 

        # Startup updating DNS
        $uri = 'https://freedns.afraid.org/dynamic/update.php?{0}' -f $freeDnsDomainId
        # https://stackoverflow.com/a/38054505
        $response = Invoke-WebRequest -URI $uri -UseBasicParsing
        $logMessage = 'request to {0} successfully with status code: {1}, content: {2}' -f $uri, $response.StatusCode, $response.Content.Trim()

        $logFileName = '{0}-{1}.txt' -f $logFilePrefix, $utcNow.ToString($dateFormat)
        'Dynamic DNS updated at: {0}, {1}' -f $utcNow.ToString('yyyy-MM-ddTHH:mm:ss.fffZ'), $logMessage `
            | Out-File (Join-Path -Path $logDirectory -ChildPath $logFileName) -Append
    }
    catch {
        $_.Exception | Out-File (Join-Path -Path $logDirectory -ChildPath 'internal-log.txt')
    }
}

# Uncomment for running without set a schedule 
# & $command; return;

# Start a task and then repeat every 30 minutes
$action = New-ScheduledTaskAction -Execute "PowerShell.exe" -Argument "-Command & { $command }" 
$repetitionIntervalMinutes = 30 
$trigger = New-ScheduledTaskTrigger `
    -Once `
    -At (Get-Date) `
    -RepetitionInterval (New-TimeSpan -Minutes $repetitionIntervalMinutes) `
    -RepetitionDuration (New-TimeSpan -Days (365 * 20))

$trigger # Output value for debugging purpose

$user = "NT AUTHORITY\SYSTEM" # Specify the account to run the script
Register-ScheduledTask `
    -TaskName "UpdateDynamicDNS" `
    -Trigger $trigger `
    -User $user `
    -Action $action `
    -RunLevel Highest `
    -Force
