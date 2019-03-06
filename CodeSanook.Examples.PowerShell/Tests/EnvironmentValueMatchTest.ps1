<#
    usage
    Invoke-Pester -Script @{ Path =  '.\EnvironmentValueMatchTest.ps1'; Parameters = @{ EnvironmentName = 'LIVE' } }
#>
param([Parameter(Mandatory = $true)] [string] $EnvironmentName)

$webConfig = [xml](Get-Content -Path (Join-Path -Path $PSScriptRoot -ChildPath "web.config"))
$apiConfig = [xml](Get-Content -Path (Join-Path -Path $PSScriptRoot -ChildPath "web.config"))

function Get-NodeValue {
    param(
        [Parameter(Mandatory = $true)] [xml] $Xml, 
        [Parameter(Mandatory = $true)] [string] $ConfigNode 
    )
    $node = Select-Xml -Xml $Xml -XPath $ConfigNode | Select-Object -First 1 -ExpandProperty Node
    $node.value
}

function Test-Endpoint {
    param(
        $EndPoints,
        $Xml
    )

    It "Validate `"<node>`" configuration in $EnvironmentName should be matched" -TestCases $EndPoints {
        param ($node, $live, $test)

        $expectedValue, $actutalValue = @(
            $PSBoundParameters[$EnvironmentName]
            Get-NodeValue -ConfigNode $node -Xml $Xml
        )
        $actutalValue | Should Be $expectedValue
    }
}

Describe "Pester test endpoint" {
    Context "Web, API, AutoTicket, CronJob" {
        $endpoints = @(
            @{ 
                node = "/configuration/connectionStrings/add[@name='DefaultConnection']/@connectionString"
                live = "Data Source=(LocalDb)\v11.0;AttachDbFilename=|DataDirectory|\aspnet-WebApplication45-20140804053515.mdf;Initial Catalog=aspnet-WebApplication45-20140804053515;Integrated Security=True"
                test = ""
            }
            @{ 
                node = "/configuration/connectionStrings/add[@name='NHibernate']/@connectionString"
                live = "Data Source=(LocalDb)\v11.0;AttachDbFilename=|DataDirectory|\aspnet-WebApplication45-20140804053515.mdf;Initial Catalog=aspnet-WebApplication45-20140804053515;Integrated Security=True"
                test = ""
            }
        )

        @($webConfig, $apiConfig) | ForEach-Object {
            Test-Endpoint -EndPoints $endpoints -Xml $_
        }
    }

    Context "Web, API" {
        $endpoints = @(
            @{ 
                node = "/configuration/connectionStrings/add[@name='DefaultConnection']/@connectionString"
                live = "Data Source=(LocalDb)\v11.0;AttachDbFilename=|DataDirectory|\aspnet-WebApplication45-20140804053515.mdf;Initial Catalog=aspnet-WebApplication45-20140804053515;Integrated Security=True"
                test = ""
            }
        )
        @($webConfig, $apiConfig) | ForEach-Object {
            Test-Endpoint -EndPoints $endpoints -Xml $_
        }
    }
}