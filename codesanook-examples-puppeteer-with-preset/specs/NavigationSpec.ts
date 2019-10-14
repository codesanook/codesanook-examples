declare let page;

describe('navigate to a website', () => {
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

    it('should go to codesanook.com', async () => {
        await page.goto('https://www.codesanook.com');
        let pageTitle = await page.title();
        expect(pageTitle).toBe('CodeSanook - CodeSanook Blog');
    });
});
