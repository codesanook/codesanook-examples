using Codesanook.Examples.DotNetCore.Models;
using CsvHelper;
using Razor.Templating.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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

            File.WriteAllText("output.html", html);
            // Assert
            Assert.NotNull(html);
        }

        public async Task Render()
        {

            const string csvFilePath = "";
            using var reader = new StreamReader(csvFilePath);
            using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);

            var airlineCountries =
                from r in csvReader.GetRecordsAsync<CustomerAddress>()
                select new
                {
                };

            var a = await airlineCountries.ToListAsync();

        }
    }
}
