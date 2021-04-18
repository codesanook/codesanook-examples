import { chromium } from 'playwright';

declare global {
  interface Window {
    resq: {
      resq$: (componentName: string, element: HTMLElement) => any;
    }
  }
}


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

  test('should get correct value after click button inside React component', async () => {
    // Arrange
    const browser = await chromium.launch();
    const context = await browser.newContext();
    const page = await context.newPage();
    await page.goto('http://localhost:3000');

    const reactComponentName = 'App';
    const rootElementHandle = await page.waitForSelector('#root');

    const result = await rootElementHandle.evaluateHandle((node: HTMLElement, componentName) => {
      const component = window.resq.resq$(componentName, node);
      return component.node[1];
    }, reactComponentName);
    const tag = result.asElement();
    const button = await tag.$('button');

    // Actual
    await button.click();

    // Assert 
    const counter = await tag.$('span');
    const value = await counter.evaluate(element => element.innerText);
    expect(Number(value)).toBe(1);
    await browser.close();
  });
});
