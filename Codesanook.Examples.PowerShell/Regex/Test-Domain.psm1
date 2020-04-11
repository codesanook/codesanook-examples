function Test-ComDomain {
    <#
    .SYNOPSIS
    check if given domain is .com domain
    
    .PARAMETER Domain
    domain to test if .com
    
    .EXAMPLE
    Test-ComDomain -Domain "jetabroad.com" #return true
    Test-ComDomain -Domain "jetabroad.com.sg" #return false
    #>
    param([Parameter(Mandatory = $true)]
        [string]
        $Domain
    )

    <#
    description of valid domain
    - letters (abc) 
    - numbers (123) 
    - and dashes/hyphens (-) 
    - spaces are not allowed 
    - can't begin or end with a dash
    - multiple dashes right next to each are allowed
    #>
    $pattern = "^[^-][0-9A-Za-z-]*\.com$"
    $match = [regex]::Match($Domain, $pattern)

    $match.Success
}

Export-ModuleMember Test-ComDomain