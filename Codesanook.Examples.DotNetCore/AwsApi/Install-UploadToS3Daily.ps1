<#
.SYNOPSIS
    Install a Windows Task Scheduler to upload a file to AWS S3 at a specific time daily 
    To use this script, please install AWS Tools for PowerShell https://sdk-for-net.amazonwebservices.com/latest/AWSToolsAndSDKForNet.msi
    You may need to run launch a PowerShell session as administrator
    Credit Admin Pong codesanook
#>

# Just in case you need it
function Disable-SynchronizeTimeZone {
<#
.SYNOPSIS
    When you create a trigger with New-ScheduledTaskTrigger Cmdlet, 
    the time you specify is converted to UTC time and saved as a string in the trigger's StartBoundary property.
    To specify a local time, we need to convert this date back into local time and remove the 'Z'; 
    Credit: 
        https://social.technet.microsoft.com/Forums/en-US/e1dad780-6a99-44f2-9688-041e7026854b/quotsynchronize-across-time-zonesquot-scheduled-task-option-and-newscheduledtasktrigger?forum=winserverpowershell
        https://www.thecliguy.co.uk/2020/02/09/scheduled-task-trigger-synchronize-across-time-zones/
#>
    param(
        [parameter(ValueFromPipeline)] 
        [CimInstance] $trigger 
    )
    $newTrigger = $trigger.Clone()
    $newTrigger.StartBoundary = [DateTime]::Parse($trigger.StartBoundary).ToLocalTime().ToString("s")
    $newTrigger
}

# Create a command as script block 
$command = {
    # *** Since we pass $command to task action, we need to use single quote for all string values.

    # Set your AWS API key that has write to S3 permission
    # You can attach the IAM instance profile to the EC2 instance so you don't need put API key here (more secure).
    $awsAccessKeyId = ''
    $awsSecretAccessKey = ''
    $awsRegion = 'ap-southeast-1' # https://docs.aws.amazon.com/general/latest/gr/rande.html#regional-endpoints

    $inputFile = 'c:/local-sample.csv' # An absolute path of a file on your local machine
    $bucketName = '' # Set to existing S3 bucket name

    # fileKey is a key that will be used to identify the object in S3. 
    # It is an optional and can be inferred from the -File parameter.
    $fileKey = 'csv/sample.csv'

    $contentType = 'text/csv'
    $logPath = 'c:/logs/task-error.txt'

    # Note
    # All ACL options
    # https://docs.aws.amazon.com/sdkfornet/v3/apidocs/index.html?page=S3/TS3S3CannedACL.html&tocid=Amazon_S3_S3CannedACL
    # StorageClass options are STANDARD, REDUCED_REDUNDANCY AND STANDARD_IA.
    # Reduced Redundancy (RRS) storage class is designed for noncritical.

    # Write-S3Object reference https://docs.aws.amazon.com/powershell/latest/reference/items/Write-S3Object.html
    try {
        Write-S3Object `
            -File $inputFile `
            -BucketName $bucketName `
            -Key $fileKey `
            -ContentType $contentType `
            -CannedACLName PRIVATE `
            -Region $awsRegion `
            -StorageClass REDUCED_REDUNDANCY `
            -AccessKey $awsAccessKeyId `
            -SecretKey $awsSecretAccessKey
    } catch {
        if(-not (Test-Path -Path $logPath)){
            New-Item -ItemType File -Path $logPath -Force
        }

        Get-Item -Path $logPath | Add-Content -Value $_.Exception 
        throw
    }
}

# For testing only
# PowerShell.exe -Command $command 

$action = New-ScheduledTaskAction -Execute "PowerShell.exe" -Argument "-Command $command" 
$startTime = "11:00pm" # Your local time

$trigger = New-ScheduledTaskTrigger -At $startTime -Daily
$user = "NT AUTHORITY\SYSTEM" # Specify the account to run the script.

Register-ScheduledTask `
    -TaskName "Upload a file to AWS S3 daily" `
    -Trigger $trigger `
    -User $user `
    -Action $action `
    -RunLevel Highest `
    -Force

