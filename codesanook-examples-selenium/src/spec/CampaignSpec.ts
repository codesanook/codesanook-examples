import { Builder, By, Key, Capabilities, until, WebDriver } from 'selenium-webdriver';
import * as chrome from 'selenium-webdriver/chrome';
const path = require('chromedriver').path;

describe("open starbucksthcampaign.com", () => {

    let driver: WebDriver = null;
    beforeEach(async () => {
        //create service
        let service = new chrome.ServiceBuilder(path);
        driver = await new Builder()
            .withCapabilities(Capabilities.chrome())
            .setChromeService(service)
            .setChromeOptions(
                new chrome.Options()
                    .setMobileEmulation({ deviceName: "iPhone X" })
                    .addArguments("--incognito")

            )
            .build();
    });

    it("should show start game button", async () => {
        await driver.get("https://starbucksthcampaign.com/c/quiz_2018_summer_3");
        let startButtonSelector = ".the-game img[alt='Start Game']";
        let startButton = await driver.findElement(By.css(startButtonSelector));
        await driver.wait(until.elementIsVisible(startButton), 10000);

    });

    afterEach(async () => {
        await driver.quit();
    })
});