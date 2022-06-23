using System.Diagnostics;
using Tmds.Utils;

namespace Codesanook.Examples.Playwright
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            if (ExecFunction.IsExecFunctionCommand(args))
            {
                return ExecFunction.Program.Main(args);
            }


            // else
            // {
            //     //await ExecFunction.RunAsync(() => Console.WriteLine("Hello world!"));
            //     var task = Task.Run(() =>
            //     {
            //         ExecFunction.Run(() =>
            //         {
            //             Console.WriteLine("Hello world!");
            //         });

            //     });

            //     await Task.WhenAll(task);

            //     return 0;
            // }

            // ExecFunction.IsExecFunctionCommand(args);
            // ExecFunction.Run(() => Console.WriteLine("Hello world!"));
            // return 0;

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var productSearcher = new ProductSearcher();
            var searchResult = await productSearcher.SearchProductsAsync(
                new[]{
                    "Sony MZ-N920",
                    "Sony MZ-N910",
                    "Sony MZ-NE810",
                    "Sony MZ-E500",
                    "Aiwa AM-F80",
                    "Aiwa AM-NX1",
                    "Sony MZ-R37",
                    "Sony MZ-RH1"
                }
            );

            foreach (var product in searchResult.SelectMany(p => p))
            {
                Console.WriteLine(product);
            }

            Console.WriteLine($"product count {searchResult.Count}");

            stopWatch.Stop();
            Console.WriteLine($"Operation took {stopWatch.Elapsed.TotalSeconds} seconds");
            return 0;
        }
    }
}
