import { chromium } from 'playwright';
import { RESQNode } from 'resq';

declare global {
  interface Window {
    resq: {
      resq$: (componentName: string, element: HTMLElement) => RESQNode
    }
  }
}

// Uppercase name for a test suite
describe('Page with React component', () => {
  test('should get correct value after click button inside React component', async () => {
    // Arrange
    const browser = await chromium.launch();
    const context = await browser.newContext();
    const page = await context.newPage();
    await page.goto('http://localhost:3000');

    // React Element Selector Query (RESQ) helps us query React components and children by component name or HTML selector.
    // We need to have it on a page that we are going to test.
    // Therefore, we add req script to that page.
    // More info https://github.com/baruchvlz/resq
    await page.addScriptTag({ path: require.resolve('resq') });

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