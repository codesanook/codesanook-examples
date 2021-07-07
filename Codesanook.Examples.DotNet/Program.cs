using System;

namespace Codesanook.Examples.DotNetCore
{
    public class Program
    {
        public static int Main(string[] args)
        {
            const int value = 1;
            Environment.Exit(2);

            // No value to the console because we already called Environment.Exit
            Console.WriteLine($"current value {value}");
            return value;
        }
    }
}
