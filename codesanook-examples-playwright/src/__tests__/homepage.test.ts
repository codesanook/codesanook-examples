import { chromium } from 'playwright';

// Uppercase name for a test suite
describe('Homepage', () => {

  // Lowercase name for a test case
  test('should launch homepage with expected title', async () => {
    const browser = await chromium.launch();
    const context = await browser.newContext();

    const page = await context.newPage();
    await page.goto('https://todomvc.com');

    const pageTitle = await page.title();
    expect(pageTitle).toBe('TodoMVC');
    await browser.close();
  });
});
