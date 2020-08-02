using Codesanook.Examples.Core.Models;
using CsvHelper.Configuration;

namespace Codesanook.Examples.DotNetCore.RazorViewEngine
{
    public class ShippingAddressItemMap : ClassMap<ShippingAddressItem>
    {
        public ShippingAddressItemMap()
        {
            // Index of column starts from 0
            // Alternatively you can use [Index()] attribute
            Map(x => x.FullName).Index(2);
            Map(x => x.MobilePhoneNumber).Index(3);
            Map(x => x.Address).Index(4);
            Map(x => x.NumberOfOrderedItems).Index(5);
        }
    }
}
