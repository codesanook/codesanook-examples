using System;

namespace Codesanook.Examples.DotNet.Orm.DapperExamples
{
    public class Office
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }
        public string BranchName { get; set; }
        public DateTime CreatedDateUtc { get; set; }
    }
}
