using Codesanook.Examples.Core.Models;
using CsvHelper;
using Razor.Templating.Core;
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

        [Fact]
        public async Task RenderShippingAddressTemplate_ValidInput_RenderCorrectly()
        {
            // Copy a file to output directory
            // https://stackoverflow.com/a/15851689/1872200
            const string receiverAddressFilePath = "RazorViewEngine/ReceiverAddresses.csv";

            using var reader = new StreamReader(receiverAddressFilePath);
            using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            csvReader.Configuration.HasHeaderRecord = true;
            csvReader.Configuration.RegisterClassMap<ShippingAddressItemMap>();

            var receiverAddresses = await csvReader.GetRecordsAsync<ShippingAddressItem>()
                .ToArrayAsync();

            const string senderAddressFilePath = "RazorViewEngine/SenderAddress.txt";
            var senderAddress = File.ReadAllText(senderAddressFilePath);
            var shippingAddress = new ShippingAdress()
            {
                SenderAddress = senderAddress,
                RecieverAddresses = receiverAddresses
            };

            var html = await RazorTemplateEngine.RenderAsync(
                "/Views/ShippingAddressTemplate.cshtml", 
                shippingAddress
            );

            File.WriteAllText("ShippingAddresses.html", html);
        }
    }
}
