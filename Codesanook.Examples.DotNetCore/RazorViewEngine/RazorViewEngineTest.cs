using Codesanook.Examples.Core.Models;
using CsvHelper;
using ICSharpCode.SharpZipLib.Zip;
using Razor.Templating.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace Codesanook.Examples.DotNetCore.RazorViewEngine
{
    // This test requires Razor.Templates and set as Microsoft.NET.Sdk.Razor
    // https://medium.com/@soundaranbu/render-razor-view-cshtml-to-string-in-net-core-7d125f32c79
    public class RazorViewEngineTest
    {
        private static readonly Regex pattern = new Regex(
            @".+(?<zipCode>\d{5})",
            RegexOptions.Compiled | RegexOptions.Singleline
        ); 

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

            var receiverAddresses =
                from a in
                csvReader.GetRecordsAsync<ShippingAddressItem>()
                let zipCode =GetZipCode(a.Address)
                select new ShippingAddressItem()
                {
                    FullName = a.FullName,
                    MobilePhoneNumber = a.MobilePhoneNumber,
                    Address = a.Address.Replace(zipCode, string.Empty),
                    ZipCode = zipCode,
                    NumberOfOrderedItems = a.NumberOfOrderedItems,
                };

            const string senderAddressFilePath = "RazorViewEngine/SenderAddress.txt";
            var senderAddress = File.ReadAllText(senderAddressFilePath);
            var shippingAddress = new ShippingAdress()
            {
                SenderAddress = senderAddress,
                RecieverAddresses = await receiverAddresses.ToArrayAsync()
            };

            var html = await RazorTemplateEngine.RenderAsync(
                "/Views/ShippingAddressTemplate.cshtml",
                shippingAddress
            );

            File.WriteAllText("ShippingAddresses.html", html);
        }

        private string GetZipCode(string address)
        {
            var match = pattern.Match(address);
            if (match.Success)
            {
                return match.Groups["zipCode"].Value;
            }

            throw new InvalidOperationException(
                $"Cannot get zip code from an address with value {address}"
            );
        }
    }
}
