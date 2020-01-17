import * as Puppeteer from 'puppeteer-ie'
import { Page, Browser } from 'puppeteer'

jest.setTimeout(60 * 1000);

describe("navigation", () => {
    it("should go to codesanook.com home page", async () => {
        const browser: Browser = await Puppeteer.launch({
            headless: false,
        });

        const page: Page = await browser.newPage();
        await page.goto("https://www.codesanook.com");
        await page.setViewport({
            width: 1280,
            height: 800
        });
        await page.screenshot({ path: 'codesanook.png' });
        await page.waitFor(10 * 1000);
        await browser.close();
    });
});
