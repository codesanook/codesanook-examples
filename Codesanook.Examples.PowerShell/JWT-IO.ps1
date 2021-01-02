# Simulate jwt.io tool in PowerShell
# Credit https://www.reddit.com/r/PowerShell/comments/8bc3rb/generate_jwt_json_web_token_in_powershell/
# https://docs.microsoft.com/en-us/powershell/scripting/developer/cmdlet/approved-verbs-for-windows-powershell-commands?view=powershell-7.1#new-vs-set

function ConvertTo-Base64Url {
    param(
        [Parameter(Mandatory = $true)] [System.Byte[]] $inputData
    )
    <#
    Base64URL

    Base64URL is a modification of the main Base64 standard, the purpose of which is the ability to use the encoding result as filename or URL address. 
    The Base64URL is described in RFC 4648 section 5, where it is mentioned that the standard Base64 alphabet contains invalid characters for URLs and filenames.
    The first problem is that the main standard uses "+" as the 62rd character and "=" as padding character. 
    Both characters have a special meaning in the URI address: "+" is interpreted as space, while "=" is used to send data via query string as "key=value" pair. 
    As you understand, using these symbols may lead to various errors.
    The second problem, the main standard uses "/" as the 63rd character, which both for URL addresses and for file system locations, represents the directory separator. 
    Therefore in this case errors are guaranteed.

    To avoid the errors above, it was proposed to use a "safe alphabet" for URL addresses and filenames. 
    Thus, the Base64URL was born. It uses the same algorithm as the main standard, but differs in the following:
    - Replaces "+" by "-" (minus)
    - Replaces "/" by "_" (underline)
    - Does not require a padding character (=)
    - Forbids line separators

    From: 
    https://base64.guru/standards/base64url

    REF:
    - RFC 4648 section 5 https://tools.ietf.org/html/rfc4648#section-5
#>
    # input of -relace is regualar expression
    [Convert]::ToBase64String($InputData) `
        -replace '=', '' `
        -replace '\+', '-' `
        -replace '/', '_'
}

# Grab Unix Epoch Timestamp and add desired expiration.
function Get-Expiry(
    [Parameter(Mandatory = $true)] [int] $ValidforSeconds
) {
    [int][double]::parse((Get-Date -Date $((Get-Date).addseconds($ValidforSeconds).ToUniversalTime()) -UFormat %s)) 
}

function New-JWT {
    param(
        [Parameter(Mandatory = $true)] [hashtable] $Header,
        [Parameter(Mandatory = $true)] [hashtable] $Payload,
        [Parameter(Mandatory = $true)] [ValidateSet("HS256", "HS384", "HS512")] [string] $Algorithm,
        [Parameter(Mandatory = $true)] $SecretKey,
        [Switch] $SecretBase64Encoded = $false
    )

    $headerJson = $Header | ConvertTo-Json -Compress
    $payloadJson = $Payload | ConvertTo-Json -Compress

    $headerJsonBase64 = ConvertTo-Base64Url -InputData ([System.Text.Encoding]::UTF8.GetBytes($headerJson))
    $payloadJsonBase64 = ConvertTo-Base64Url -InputData ([System.Text.Encoding]::UTF8.GetBytes($payloadJson))
    $ToBeSigned = "$headerJsonBase64.$payloadJsonBase64"

    $SigningAlgorithm = switch ($Algorithm) {
        "HS256" { New-Object System.Security.Cryptography.HMACSHA256 }
        "HS384" { New-Object System.Security.Cryptography.HMACSHA384 }
        "HS512" { New-Object System.Security.Cryptography.HMACSHA512 }
    }

    $SigningAlgorithm.Key = [System.Text.Encoding]::UTF8.GetBytes($SecretKey)
    $signature = $SigningAlgorithm.ComputeHash([System.Text.Encoding]::UTF8.GetBytes($ToBeSigned))
    $signatureBase64 = ConvertTo-Base64Url -InputData $signature

    # Prepare output
    "$headerJsonBase64.$payloadJsonBase64.$signatureBase64"
}

$header = @{ alg = 'HS256'; typ = 'JWT' }
$payload = @{ sub = "1234567890"; name = 'John Doe'; iat = 1516239022 }
$secretKey = "your-256-bit-secret"
New-JWT -Header $header -Payload $payload -Algorithm $header.alg -SecretKey $secretKey

<#
FYI
    The option secret base64 encoded on https://jwt.io means that the secret you paste into the input field is base64 encoded an therefore needs to be decoded before it is applied. 
    Then jwt.io makes this extra step to decode the secret first.
    jwt.io indeed produces the same hash in both cases:
    secret = 'abc' and secret base64 encoded= no 
    or
    secret = 'YWJj' and secret base64 encoded = yes (abc to base64URl = YWJj)
    https://stackoverflow.com/a/52388404/1872200
#>
