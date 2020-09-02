using Codesanook.Examples.Core.Models;
using CsvHelper;
using Razor.Templating.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
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

        public RazorViewEngineTest() => RazorTemplateEngine.Initialize();

        [Fact]
        public async Task RenderShippingAddressTemplate_ValidInput_RenderCorrectly()
        {
            // Copy a file to output directory
            // https://stackoverflow.com/a/15851689/1872200

            // Example of CSV content 
            // Time, Image ,Full name, Mobile phone number, Address, Number of ordered items,
            // 7/8/2020 19:37:02,abc.jpg,first name last name,0812345678,"9999 Address 102400", 2 
            const string receiverAddressFilePath = "RazorViewEngine/ReceiverAddresses.csv";
            using var reader = new StreamReader(receiverAddressFilePath);
            using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);

            csvReader.Configuration.HasHeaderRecord = true;
            csvReader.Configuration.RegisterClassMap<ShippingAddressItemMap>();

            var receiverAddresses = CreateShippingAddressItems(csvReader);
            const string senderAddressFilePath = "RazorViewEngine/SenderAddress.txt";
            var senderAddress = File.ReadAllLines(senderAddressFilePath)
                .Aggregate(
                    new StringBuilder(),// seed
                    (accumulated, currentValue) => accumulated.Append($"{currentValue}<br />")
                );

            var shippingAddress = new ShippingAdress()
            {
                SenderAddress = senderAddress.ToString(),
                RecieverAddresses = await receiverAddresses.ToArrayAsync()
            };

            var html = await RazorTemplateEngine.RenderAsync(
                "/Views/ShippingAddressTemplate.cshtml",
                shippingAddress
            );

            // The output file is in Codesanook.Examples.DotNetCore/bin/Debug/netcoreapp3.1/win-x64/ShippingAddresses.html
            File.WriteAllText("ShippingAddresses.html", html);
            Assert.NotEmpty(html);
        }

        private IAsyncEnumerable<ShippingAddressItem> CreateShippingAddressItems(CsvReader csvReader)
        {
            return
                from a in csvReader.GetRecordsAsync<ShippingAddressItem>()
                let zipCode = GetZipCode(a.Address)
                select new ShippingAddressItem()
                {
                    FullName = a.FullName,
                    MobilePhoneNumber = a.MobilePhoneNumber,
                    Address = a.Address.Replace(zipCode, string.Empty),
                    ZipCode = zipCode,
                    ReferenceId = a.ReferenceId
                };
        }

        private string GetZipCode(string address)
        {
            var match = pattern.Match(address);

            return match.Success
                ? match.Groups["zipCode"].Value
                : throw new InvalidOperationException(
                    $"Cannot get zip code from an address with value {address}"
                );
        }
    }
}
