$path = "$($env:USERPROFILE)\AppData\Local\Temp\*" 
Remove-Item -Path $path -Recurse -Force