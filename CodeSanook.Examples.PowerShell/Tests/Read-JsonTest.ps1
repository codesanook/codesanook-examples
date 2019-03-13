
# https://docs.microsoft.com/en-us/powershell/module/microsoft.powershell.core/about/about_preference_variables?view=powershell-6#debugpreference
$DebugPreference = "Continue"

Describe "Read JSON" {
    It "should read JSON and random pick name correctly" {
        $filePath = Join-Path -Path $PSScriptRoot -ChildPath "names.json"
        $names = [System.Collections.ArrayList](Get-Content $filePath | ConvertFrom-Json)
        $randomNames = New-Object -TypeName System.Collections.ArrayList

        while ($names.Count -gt 1) {
            $random = Get-Random -InputObject $names
            $names.Remove($random);
            $randomNames.Add($random);
        }

        $randomNames.Add($names[0]);
        Write-Debug (($randomNames | Select-Object -First 7) -join ", ")
    }
}