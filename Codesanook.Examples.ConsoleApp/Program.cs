﻿using System;
using System.Threading.Tasks;

namespace Codesanook.Examples.ConsoleApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            return Task.Delay(1000);
            Console.WriteLine("Hello World!");
        }
    }
}