
$liveEndpoints = @(
    @{ a = 1; b = 2; expectedResult = 3 }
    @{ a = 5; b = 8; expectedResult = 13 }
)

$uatEndpoints = @(
    @{  a = 1; b = 2; expectedResult = 3 }
    @{ a = 5; b = 8; expectedResult = 13 }
)


Describe -Tag "LIVE" -Name "Pester basic command" -Fixture {
    It "LIVE Adds <a> and <b> should return <expectedResult>" -TestCases $liveEndpoints {
        param ( $a, $b, $expectedResult)
        $a + $b | Should Be $expectedResult
    }
}

Describe -Tag "UAT" -Name "Pester basic command" -Fixture {
    It "UAT Adds <a> and <b> should return <expectedResult>" -TestCases $uatEndpoints {
        param ( $a, $b, $expectedResult)
        $a + $b | Should Be $expectedResult
    }
}

#"appsetting",