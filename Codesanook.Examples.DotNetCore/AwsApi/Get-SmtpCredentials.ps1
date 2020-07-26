param(
    [Parameter(Mandatory=$true)] [string] $Key,
    [Parameter(Mandatory=$true)] [string] $Region # us-east-2 for US East (Ohio)	
)

# Additional information to obtain SMTP from API secret key 
# Original script from https://gist.github.com/damusix/c12400ee0ccb7e56351619ae2b19a303
# https://docs.aws.amazon.com/ses/latest/DeveloperGuide/smtp-credentials.html
# https://docs.aws.amazon.com/ses/latest/DeveloperGuide/control-user-access.html#iam-and-ses-examples-email-sending-actions

$date = "11111111"
$service = "ses"
$terminal = "aws4_request"
$message = "SendRawEmail"
$versionInBytes = 0x04

function HmacSha256($text, $key2) {
    $hmacsha = New-Object System.Security.Cryptography.HMACSHA256
    $hmacsha.key = $key2
    $hmacsha.ComputeHash([Text.Encoding]::UTF8.GetBytes($text))
}

$signature = [Text.Encoding]::UTF8.GetBytes("AWS4" + $Key)
$signature = HmacSha256 $date $signature
$signature = HmacSha256 $Region $signature
$signature = HmacSha256 $service $signature
$signature = HmacSha256 $terminal $signature
$signature = HmacSha256 $message $signature
$signatureAndVersion = [System.Byte[]]::CreateInstance([System.Byte], $signature.Length + 1)
$signatureAndVersion[0] = $versionInBytes
$signature.CopyTo($signatureAndVersion, 1)
$smtpPassword = [Convert]::ToBase64String($signatureAndVersion)

$smtpPassword
