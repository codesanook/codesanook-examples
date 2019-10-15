import { Page } from 'puppeteer';
declare let page: Page;
declare let jestPuppeteer;

describe('navigate to a website', () => {
    const timeoutInSecond = 60;

    beforeAll(async () => {
        jest.setTimeout(timeoutInSecond * 1000);
    });

    beforeEach(async () => {
        console.log('Begin before each');
        await jestPuppeteer.resetBrowser();
        console.log('End before each');
    });

    afterEach(() => {});

    afterAll(() => {});

    it('should go to codesanook.com', async () => {
        console.log('Start testing');
        await page.goto('https://www.codesanook.com');
        await page.waitFor(1000);
        let pageTitle = await page.title();
        expect(pageTitle).toBe('CodeSanook - CodeSanook Blog');
    });

    it('should go to reactjs.org', async () => {
        console.log('Start testing');
        await page.goto('https://reactjs.org');
        await page.waitFor(1000);
        let pageTitle = await page.title();
        expect(pageTitle).toBe(
            'React – A JavaScript library for building user interfaces'
        );
    });
});
