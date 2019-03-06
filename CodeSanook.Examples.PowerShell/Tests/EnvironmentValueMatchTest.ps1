


function Get-ActualValue {

    $Xml = [xml]@"
<?xml version="1.0" encoding="utf-8"?>
<Book>
  <projects>
    <project name="Book1" date="2009-01-20">
      <editions>
        <edition name="a" language="English">En.Book1.com</edition>
        <edition name="b" language="German">Ge.Book1.Com</edition>
        <edition name="c" language="French">Fr.Book1.com</edition>
        <edition name="d" language="Polish">Pl.Book1.com</edition>
      </editions>
    </project>
  </projects>
</Book>
"@


    Select-Xml -xml $Xml -XPath "//edition[@name='a']/@language" | ForEach-Object { $_.node.Value }
    Select-Xml -xml $Xml -XPath "//edition[@name='b']" | ForEach-Object { $_.node.InnerText }

    return ""
}

Describe -Tag "LIVE" -Name "Pester basic command" -Fixture {
    Context "Web and API" {
        $endpoints = @(
            @{ configNode = "connectionString"; expectedLive = "live"; expectedTest = "test"  }
            @{ configNode = "connectionString"; expectedLive = "live"; expectedTest = "test" }
        )

        It "Validate Web configuration" -TestCases $endpoints {
            param ($configNode, $expectedLive, $expectedTest)
            $environmentName = "live"

            $expectedValue, $actutalValue = @($PSBoundParameters["expected$environmentName"], (Get-ActualValue))


            #$parameterList = (Get-Command -Name $CommandName).Parameters;
            #$parameterList | ForEach-Object {
            #    (Get-Variable -Name $_.Values.Name -ErrorAction SilentlyContinue)
            #}

            $expectedValue -eq $actutalValue | Should Be $true
        }

    }

    Context "Web" {
        $endpoints = @(
            @{ configNode = "connectionString"; expectedLive = ""; expectedTest = ""  }
            @{ configNode = "connectionString"; expectedLive = ""; expectedTest = "" }
        )
        It "Validate Web configuration" -TestCases $endpoints {
            param ( $configNode, $expectedLive, $expectedTest)
            $expectedLive -eq $expectedTest | Should Be $true
        }
    }

    Context "API" {
        $endpoints = @(
            @{ configNode = "connectionString"; expectedLive = ""; expectedTest = ""  }
            @{ configNode = "connectionString"; expectedLive = ""; expectedTest = "" }
        )

        It "Validate Web configuration" -TestCases $endpoints {
            param ( $configNode, $expectedLive, $expectedTest)
            $expectedLive -eq $expectedTest | Should Be $true
        }
    }
}

function GetExepctedValueForEnvironment {
    param(
        [hashtable] $TestCaseData,
        [string] $EnvironmentName
    )

    $TestCaseData["expected$EnvironmentName"] #Case insensitive key
}
#Example of useage Invoke-Pester -Tag LIVE