$thaiLanguage = "Thai"
$otherLanguages = "English", "French", "Chinese", "Japanese"

function Print-AllLanguages {
    param(
        [Parameter(Mandatory = $true)] [string[]] $languages
    )

    Write-Host "`$languages.Length: $($languages.Length)"
  
    $languages | ForEach-Object {
        Write-Host "memeber value: $_"
    }  
}

Print-AllLanguages (@($thaiLanguage) + $otherLanguages)