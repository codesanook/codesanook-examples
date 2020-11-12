using System.Collections.Generic;
using Xunit;

namespace Codesanook.Examples.DotNetCore.Collections
{
    public class Student { public string Name { set; get; } }

    public class DictionaryTest
    {
        [Fact]
        public void CreateDictionary_WithManyWayToInitialize_InvalidInitializeThrowException()
        {
            var s1 = new Dictionary<int, Student>
            {
                { 1,  new Student { Name = "a1" } },
                { 2, new Student { Name = "a2"} },
            };

            var s2 = new Dictionary<int, Student>
            {
                [1] = { },
                [2] = { },
            };

            // C# 9 https://devblogs.microsoft.com/dotnet/welcome-to-c-9-0/#target-typed-new-expressions
            var s3 = new Dictionary<int, Student>
            {
                [1] = new(),
                [2] = new(),
            };

            var s4 = new Dictionary<int, Student>
            {
                [1] = { Name = "a1" },
                [2] = { Name = "a2" },
            };

            var s5 = new Dictionary<int, Student>
            {
                [1] = new(),
                [2] = new(),
                [1] = { Name = "a1" },
                [2] = { Name = "a2" },
            };
        }
    }
}
