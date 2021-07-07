using System;

namespace Codesanook.Examples.DotNetCore.Models
{
    public class Entity
    {
        public int Id { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public bool IsDeleted { get; set; }
    }
}
