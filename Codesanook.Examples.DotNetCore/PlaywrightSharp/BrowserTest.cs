using PlaywrightSharp;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Codesanook.Examples.DotNetCore.PlaywrightSharp
{
    // Before running test cases

    // Run the following two commands in a terminal to avoid having the await Playwright.InstallAsync(); code.
    // The first command take several minutes
    // dotnet tool install playwright-sharp-tool -g 
    // playwright-sharp install-browsers

    // Then Install-Package PlaywrightSharp
    public class BrowserTest : IAsyncLifetime
    {
        private const string filePath = "./screenshot.png"; // relative to bin/debug

        public Task InitializeAsync()
        {
            File.Delete(filePath); // Delete an existing screenshot file if exist
            return Task.CompletedTask;
        }

        public Task DisposeAsync() => Task.CompletedTask;

        [Fact]
        public async Task NewPageAsync_ValidUrl_ScreenshotSaved()
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Webkit.LaunchAsync();

            var page = await browser.NewPageAsync();
            await page.GoToAsync("https://www.codesanook.com");
            await page.ScreenshotAsync(path: filePath);

            Assert.True(File.Exists(filePath));
        }
    }
}

