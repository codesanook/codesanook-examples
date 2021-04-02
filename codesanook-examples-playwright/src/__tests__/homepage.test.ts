import { webkit } from 'playwright';
jest.setTimeout(10000);

// Uppercase name
describe('Homepage', () => {

  // Lowercase name
  test('should launch homepage with headless mode', async () => {
    const browser = await webkit.launch();
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

  test('should launch homepage with headful mode', async () => {
    const browser = await webkit.launch({ headless: false });
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