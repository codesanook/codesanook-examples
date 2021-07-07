using System;

namespace Codesanook.Examples.DotNetCore.Orm.NHExamples
{
    public interface IAuditable
    {
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
    }
}
