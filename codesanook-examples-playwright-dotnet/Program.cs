using System.Diagnostics;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Playwright;

public class Program
{
    public static async Task Main()
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();

        await SearchAsync(
            "Sony MZ-N920",
            "Sony MZ-N910",
            "Sony MZ-NE810",
            "Sony MZ-E500",
            "Aiwa AM-F80",
            "Aiwa AM-NX1",
            "Sony MZ-R37"
        );

        stopWatch.Stop();
        Console.WriteLine($"Operation took {stopWatch.Elapsed.TotalSeconds} seconds");
    }

    private static async Task SearchAsync(params string[] keywords)
    {
        var htmlContents = await GetHtmlContentAsync(keywords);
        var products = htmlContents.Where(r => !string.IsNullOrEmpty(r.HtmlContent)).AsParallel().Select(r =>
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(r.HtmlContent);
            var links = htmlDoc.DocumentNode.SelectNodes("//mer-item-thumbnail/parent::a");

            return links.Select(l =>
            {
                var item = l.SelectSingleNode("./mer-item-thumbnail");
                return new Product
                {
                    Name = r.Keyword,
                    DetailsUrl = l.Attributes["href"].Value,
                    ShortDescription = item.Attributes["alt"].Value,
                    ImageUrl = item.Attributes["src"].Value,
                    Price = Convert.ToDecimal(item.Attributes["price"].Value)
                };
            });

        }).SelectMany(p => p).ToList();

        foreach (var product in products)
        {
            Console.WriteLine(product);
        }
    }

    private static async Task<IReadOnlyCollection<(string Keyword, string HtmlContent)>> GetHtmlContentAsync(string[] keywords)
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new() { Headless = true });
        await using var context = await browser.NewContextAsync(
            new BrowserNewContextOptions()
            {
                Locale = "ja-JP",
                UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.93 Safari/537.36"
            }
        );

        var tasks = keywords.Select(k => GetContentForEachKeywordAsync(k, context));
        var contents = await Task.WhenAll(tasks);
        return keywords.Zip(contents, (k, c) => (k, c)).ToList();
    }

    private static async Task<string> GetContentForEachKeywordAsync(string keyword, IBrowserContext context)
    {
        try
        {
            var page = await context.NewPageAsync();
            var encodedKeyword = Uri.EscapeDataString(keyword);
            var searchUrl = $"https://jp.mercari.com/search?keyword={encodedKeyword}&order=desc&sort=created_time&status=on_sale";
            await page.GotoAsync(searchUrl);
            await page.WaitForSelectorAsync("mer-item-thumbnail", new PageWaitForSelectorOptions() { Timeout = 5 * 1000 });
            return await page.ContentAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }

    public class Product
    {
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string ImageUrl { get; set; }
        public string DetailsUrl { get; set; }
        public decimal Price { get; set; }

        public override string ToString()
        {
            return (
                $"Name: {Name}" +
                $"Details URL: {DetailsUrl}" +
                $"Short description: {ShortDescription}" +
                $"Image: {ImageUrl}" +
                $"Price: {Price}"
            );
        }
    }
}
