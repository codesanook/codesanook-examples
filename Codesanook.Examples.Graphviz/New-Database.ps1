function Get-LastestVersionFilePath {
    param(
        [Parameter(Mandatory = $true)] [string] $FileName, 
        [Parameter(Mandatory = $false)] [string] $Framework
    )

    $pattern = if ($framework) { ".*packages.*$Framework.*$FileName$" } else { ".*packages.*net\d{2,3}.*$FileName$" }
    $allDllPaths = Get-ChildItem -Path "./packages" -Recurse -File
    $allDllPaths | Where-Object {
        $_.FullName -match $pattern
    } | Select-Object -ExpandProperty "FullName" | Sort-Object -Descending | Select-Object -First 1
}

$libraryName = "System.Data.SQLite"
& .\nuget.exe Install $libraryName -DependencyVersion Lowest -OutputDirectory "./packages"

# Copy all file a to target folder
$outputFolder = "./bin"
Remove-Item $outputFolder -Recurse -Force -ErrorAction Ignore
New-Item -Path $outputFolder -ItemType Directory -Force

"getting assembly path"
$assemblyPath = Get-LastestVersionFilePath -FileName "$libraryName.dll"
#[Reflection.Assembly]::LoadFrom($assemblyPath) | Out-Null
$framework = "net46"
"Framework $framework"

$file = Get-LastestVersionFilePath -FileName "$libraryName.dll" -Framework $framework
@(
    Get-LastestVersionFilePath -FileName "$libraryName.dll" -Framework $framework
) | Copy-Item -Destination $outputFolder

Get-LastestVersionFilePath -FileName "Export-SqlQuery.ps1" | Copy-Item -Destination "."
Get-LastestVersionFilePath -FileName "Export-SqlQueryModule.psm1" | Copy-Item -Destination "."
"Install successfully"


"Creating child AppDomain..."  

$appDomain = [System.AppDomain]::CreateDomain("AppDomain", $null, $null); 
$buffer = [System.IO.File]::ReadAllBytes($file); 
$assembly = $appDomain.Load($buffer); 
$attribute = $assembly.GetCustomAttributes([System.Runtime.Versioning.TargetFrameworkAttribute])

$pattern = "version=v(?<version>[\w\.]+)"
$option = [Text.RegularExpressions.RegexOptions]::IgnoreCase
$match = [Regex]::Match($attribute.FrameworkName, $pattern, $option);
$framework = $match.Groups["version"].value -replace "\.", ""
"Framework $framework"

$types = $assembly.GetTypes(); 
$types | ForEach-Object {
    $_.FullName
}

[System.AppDomain]::Unload($appDomain); 
