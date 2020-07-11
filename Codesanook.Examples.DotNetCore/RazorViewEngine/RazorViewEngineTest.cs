using Razor.Templating.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Codesanook.Examples.DotNetCore.RazorViewEngine
{
    // This test requires Razor.Templates and set as Microsoft.NET.Sdk.Razor
    // https://medium.com/@soundaranbu/render-razor-view-cshtml-to-string-in-net-core-7d125f32c79
    public class RazorViewEngineTest
    {
        public RazorViewEngineTest()
        {
            RazorTemplateEngine.Initialize();
        }

        [Fact]
        public async Task RenderAsync_ValidView_ReturnRenderedHtmlContent()
        {
            var viewData = new Dictionary<string, object>();
            viewData["Value1"] = "1";
            viewData["Value2"] = "2";
            var html = await RazorTemplateEngine.RenderAsync("/Views/ExampleView.cshtml", new { }, viewData);
            // Assert
            Assert.NotNull(html);
        }
    }
}
