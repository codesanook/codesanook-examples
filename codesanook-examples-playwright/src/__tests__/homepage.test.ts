import { chromium } from 'playwright';

// Uppercase name
describe('Homepage', () => {

  // Lowercase name
  test('should launch homepage', async () => {
    const browser = await chromium.launch();
    const context = await browser.newContext();
    const page = await context.newPage();

    // Log and continue all network requests
    page.route('**', route => {
      console.log(route.request().url());
      route.continue();
    });

    await page.goto('http://todomvc.com');
    await browser.close();
  });
});