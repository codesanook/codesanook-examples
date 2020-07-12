using System;
using System.Collections.Generic;
using System.Text;

namespace Codesanook.Examples.Core.Models
{
    public class ShippingAdress
    {
        public IReadOnlyCollection<ShippingAddressItem> RecieverAddresses { get; set; }
        public string SenderAddress { get; set; }
    }
}

