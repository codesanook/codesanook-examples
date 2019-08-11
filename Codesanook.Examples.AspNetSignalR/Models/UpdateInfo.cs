using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Codesanook.Examples.AspNetSignalR.Models
{
    public class UpdateInfo
    {
        public int OrderId { get; set; }
        public bool New { get; set; }
        public string Update { get; set; }
        public bool Finished { get; set; }
    }
}