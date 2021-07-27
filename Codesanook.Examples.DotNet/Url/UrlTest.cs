using System;
using Xunit;
using UrlHelper = Flurl.Url;

namespace Codesanook.Examples.DotNet.Url
{
    public class UrlTest
    {
        [Fact]
        public void AppendPathSegments_ValidMultiplePath_ReturnCorrectUrl()
        {
            var uriBuildler = new UriBuilder()
            {
                Scheme = "https",
                Host = "https://www.abc.com"
            };

            var path1 = "xx";
            var path2 = "yy";

            // https://www.hanselman.com/blog/UsingFlurlToEasilyBuildURLsAndMakeTestableHttpClientCallsInNET.aspx
            var url = new UrlHelper(uriBuildler.Uri);
            url.AppendPathSegments(path1, path2);

            Assert.Equal("https://www.abc.com/xx/yy", url);
        }
    }
}
