import { chromium } from 'playwright';

// Uppercase name for a test suite
describe('Homepage', () => {
  jest.setTimeout(20 * 1000);
  // Lowercase name for a test case
  test('should launch homepage with expected title', async () => {

    const browser = await chromium.launch({
      headless: true
    });

    const context = await browser.newContext();
    // Open new page
    const page = await context.newPage();
    // Go to https://staging.petpaw.com/
    await page.goto('https://staging.petpaw.com/');
    // Click text=2 >> div
    await page.click('text=2 >> div');
    // assert.equal(page.url(), 'https://staging.petpaw.com/signin');
    // Click #content-modal >> :nth-match(i, 4)
    await page.click('#content-modal >> :nth-match(i, 4)');
    // assert.equal(page.url(), 'https://staging.petpaw.com/');
    // Click text=Join Now!
    await page.click('text=Join Now!');
    // ---------------------
    await context.close();
    await browser.close();

  });
});
