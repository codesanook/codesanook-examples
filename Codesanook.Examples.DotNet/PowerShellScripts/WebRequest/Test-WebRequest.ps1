try
{
    $freeDnsDomainId = ""
    $uri = "https://freedns.afraid.org/dynamic/update.php?$freeDnsDomainId"
    $response = Invoke-WebRequest -URI $uri
    "Response body: $($response.Content)"
    "Request successfully with status code: '$($response.StatusCode)'"
}
catch
{
    $statusCode = $_.Exception.Response.StatusCode.value__
    "Request failed with status code: '$statusCode'"
}
