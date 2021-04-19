using System;
using System.Linq;

namespace Codesanook.Examples.DebugDotNet
{
    public static class Program
    {
        static void Main(string[] args)
        {
            var values = new[] { "a", "b", "c" };
            var output = values.Select((v, i) => $"index {i}: value: {v}");
            Console.WriteLine(string.Join("\n", output));
        }
    }
}
