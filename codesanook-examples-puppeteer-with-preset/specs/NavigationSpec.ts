import { Page } from 'puppeteer';
import * as path from 'path';
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

    it('should launch local index HTML file', async () => {
        // Launch page
        await page.goto(`file:${path.join(__dirname, 'index.html')}`);

        // Wait for alert show up
        const dialogPromise = new Promise<string>(resolve => {
            page.on('dialog', async dialog => {
                await page.waitFor(2000);
                await dialog.accept('OK');
                resolve(dialog.type());
            });
        });

        // Click alert
        const btnShowAlert = await page.$('#btnShowAlert');
        btnShowAlert.click();

        // Assert dialog type
        const dialogType = await dialogPromise;
        expect(dialogType).toEqual('alert');
    });
});
