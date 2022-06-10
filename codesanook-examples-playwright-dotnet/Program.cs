using System.Threading.Tasks;
using Microsoft.Playwright;

public class Program
{
    public static async Task Main()
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

        var page = await context.NewPageAsync();
        await page.GotoAsync("https://jp.mercari.com/search?keyword=iRiver-iHP&order=desc&sort=created_time");
        await page.WaitForSelectorAsync("mer-item-thumbnail");

        var html = await page.ContentAsync();
        var links = page.Locator("//mer-item-thumbnail/parent::a");
        var count = await links.CountAsync();
        for (var index = 0; index < count; index++)
        {
            var link = links.Nth(index);
            var href = await link.GetAttributeAsync("href");
            var item = link.Locator("//mer-item-thumbnail");
            var name = await item.GetAttributeAsync("item-name");
            var src = await item.GetAttributeAsync("src");

            Console.WriteLine($"link URL {href}, name {name}, image {src}");
        }

        File.WriteAllText("./page.html", html);

    }
}