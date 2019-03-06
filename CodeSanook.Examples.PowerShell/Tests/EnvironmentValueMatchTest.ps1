<#
    usage
    Invoke-Pester -Script @{ Path =  '.\EnvironmentValueMatchTest.ps1'; Parameters = @{ EnvironmentName = 'LIVE' } }
#>
param([Parameter(Mandatory = $true)] [string] $EnvironmentName)

$webConfig = [xml](Get-Content -Path (Join-Path -Path $PSScriptRoot -ChildPath "web.config"))
$apiConfig = [xml](Get-Content -Path (Join-Path -Path $PSScriptRoot -ChildPath "web.config"))
$autoTicketConfig = [xml](Get-Content -Path (Join-Path -Path $PSScriptRoot -ChildPath "web.config"))
$cronJobConfig = [xml](Get-Content -Path (Join-Path -Path $PSScriptRoot -ChildPath "web.config"))

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

        It "Validate `"<node>`" configuration for web project in $EnvironmentName should be matched" -TestCases $endpoints {
            param ($node, $live, $test)

            $expectedValue, $actutalValue = @(
                $PSBoundParameters[$EnvironmentName]
                Get-NodeValue -ConfigNode $node -Xml $webConfig
            )
            $actutalValue | Should Be $expectedValue
        }

        It "Validate `"<node>`" configuration for API project in $EnvironmentName should be matched" -TestCases $endpoints {
            param ($node, $live, $test)

            $expectedValue, $actutalValue = @(
                $PSBoundParameters[$EnvironmentName]
                Get-NodeValue -ConfigNode $node -Xml $apiConfig
            )
            $actutalValue | Should Be $expectedValue
        }

        It "Validate `"<node>`" configuration for auto ticket project in $EnvironmentName should be matched" -TestCases $endpoints {
            param ($node, $live, $test)

            $expectedValue, $actutalValue = @(
                $PSBoundParameters[$EnvironmentName]
                Get-NodeValue -ConfigNode $node -Xml $autoTicketConfig
            )
            $actutalValue | Should Be $expectedValue
        }

        It "Validate `"<node>`" configuration for cron project in $EnvironmentName should be matched" -TestCases $endpoints {
            param ($node, $live, $test)

            $expectedValue, $actutalValue = @(
                $PSBoundParameters[$EnvironmentName]
                Get-NodeValue -ConfigNode $node -Xml $cronJobConfig
            )
            $actutalValue | Should Be $expectedValue
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

        It "Validate `"<node>`" configuration for web project in $EnvironmentName should be matched" -TestCases $endpoints {
            param ($node, $live, $test)

            $expectedValue, $actutalValue = @(
                $PSBoundParameters[$EnvironmentName]
                Get-NodeValue -ConfigNode $node -Xml $webConfig
            )
            $actutalValue | Should Be $expectedValue
        }

        It "Validate `"<node>`" configuration for API project in $EnvironmentName should be matched" -TestCases $endpoints {
            param ($node, $live, $test)

            $expectedValue, $actutalValue = @(
                $PSBoundParameters[$EnvironmentName]
                Get-NodeValue -ConfigNode $node -Xml $apiConfig
            )
            $actutalValue | Should Be $expectedValue
        }


    }
}

function Get-NodeValue {
    param(
        [Parameter(Mandatory = $true)] [string]  $Xml, 
        [Parameter(Mandatory = $true)] [string]  $ConfigPath 
    )
    $node = Select-Xml -Xml $Xml -XPath $ConfigNode | Select-Object -First 1 -ExpandProperty Node
    $node.value
}