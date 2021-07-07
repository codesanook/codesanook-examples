$command = {
    try {
        & 'C:\ProgramData\chocolatey\lib\dirsyncpro\tools\DirSyncPro-1.53-Windows\DirSyncPro.exe' /analyze /nogui /quit 'C:\tools\sync-job.dsc'
        & 'C:\ProgramData\chocolatey\lib\dirsyncpro\tools\DirSyncPro-1.53-Windows\DirSyncPro.exe' /sync /nogui /quit 'C:\tools\sync-job.dsc'
    }
    catch {
        $logPath = "c:\logs"
        New-Item -Path $logPath -ItemType Directory -Force # New folder if not exist
        $_.Exception | Out-File (Join-Path -Path $logPath -ChildPath 'syncing-error.txt')
    }
}

$action = New-ScheduledTaskAction -Execute "PowerShell.exe" -Argument "-Command & { $command }" 

$repeat = [timespan]::FromHours(2)
$trigger = New-ScheduledTaskTrigger -At (Get-Date).AddMinutes(1) -Once -RepetitionInterval $repeat -RepetitionDuration ([timespan]::FromDays(365*10)) 
$user = "NT AUTHORITY\SYSTEM" # Specify the account to run the script

Register-ScheduledTask `
    -TaskName "SyncDir" `
    -Trigger $trigger `
    -User $user `
    -Action $action `
    -RunLevel Highest `
    -Force
