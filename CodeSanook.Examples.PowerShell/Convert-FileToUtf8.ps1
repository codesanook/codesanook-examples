param(
    [Parameter(Mandatory = $true)] [string] $filePath
)

$fileContent = Get-Content $filePath
$Utf8NoBomEncoding = New-Object System.Text.UTF8Encoding $False
[System.IO.File]::WriteAllLines($filePath, $fileContent, $Utf8NoBomEncoding)