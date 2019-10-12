import { launch } from "puppeteer";

declare let browser;
describe("search", () => {
    const timeoutInSecond = 60;

    beforeAll(done => {
        jest.setTimeout(timeoutInSecond * 1000);
        done();
    });

    beforeEach(async done => {
        done();
    });

    afterEach(async done => {
        done();
    });

    afterAll(async done => {
        done();
    });

    it("should go to codesanook.com", async () => {
        const browser = await launch({
            headless: false,
            defaultViewport: {
                width: 320,
                height: 568,
                deviceScaleFactor: 2
            }
        });

        const page = await browser.newPage();
        await page.goto("https://www.codesanook.com");
        await page.screenshot({ path: "codesanook.png" });
        await page.waitFor(3000);
        await browser.close();
    });
});
