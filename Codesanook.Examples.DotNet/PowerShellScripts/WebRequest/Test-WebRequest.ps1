try
{
    $uri = "https://freedns.afraid.org/dynamic/update.php?QkllTEZaSm8yQ1pnWXNQS1lPVE91dERtOjE4ODcwNDky"
    $response = Invoke-WebRequest -URI $uri
    "Response body: $($response.Content)"
    "Request successfully with status code: '$($response.StatusCode)'"
}
catch
{
    "Request failed with status code: '${.Exception.Response.StatusCode.value__}'"
}
