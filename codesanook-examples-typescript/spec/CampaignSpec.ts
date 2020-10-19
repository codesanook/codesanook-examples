import { Builder, By, Capabilities, until, WebDriver } from 'selenium-webdriver';
import * as chrome from 'selenium-webdriver/chrome';
import Question, { Choice } from '../src/models/Question';
const path: string = require('chromedriver').path;

describe('open starbucksthcampaign.com', () => {
    let driver: WebDriver;
    beforeEach(async () => {
        // Create a service
        const service = new chrome.ServiceBuilder(path);
        const capabilities = new Capabilities();
        // Normal is the default value and wait until page loaded, just explicit set a value
        // Learn more https://www.selenium.dev/documentation/en/webdriver/page_loading_strategy/
        capabilities.setPageLoadStrategy('normal'); // 

        driver = await new Builder()
            .withCapabilities(capabilities)
            .forBrowser('chrome')
            .setChromeService(service)
            .setChromeOptions(
                new chrome.Options()
                    // Add .setMobileEmulation({ deviceName: 'iPhone X' }) for a act as mobile device
                    .addArguments('--incognito')
            )
            .build();
    });

    afterEach(async () => {
        await driver.quit();
    })

    it('should go to codesanook.com', async () => {
        const url = 'https://www.codesanook.com';
        await driver.get(url);

        // We don't need this this because we already set page load strategy but just show an additional example
        const waitTimoutInSecond = 5;
        await driver.wait(async () => {
            // The DOMContentLoaded event is also fired when the readyState changes from loading to interactive.
            // https://levelup.gitconnected.com/understand-browser-readystate-and-how-to-track-the-interactivity-of-the-content-on-your-web-page-8d2802f29aa
            const readyState = await driver.executeScript<string>('return document.readyState');
            return readyState === 'complete';
        }, waitTimoutInSecond * 1000);

        const pageTitle = await driver.getTitle();
        expect(pageTitle).toBe('Codesanook - Codesanook');
    });
});

describe('open starbucksthcampaign.com', () => {
    let driver: WebDriver;
    beforeEach(async () => {

        // Create service
        const service = new chrome.ServiceBuilder(path);
        driver = await new Builder()
            .withCapabilities(Capabilities.chrome())
            .setChromeService(service)
            .setChromeOptions(
                new chrome.Options()
                    // Add .setMobileEmulation({ deviceName: 'iPhone X' }) for a act as mobile device
                    .addArguments('--incognito')
            )
            .build();
    });

    it('should be able to submit correct answers with a phone number', async () => {
        const phoneNumber = getPhoneNumber();
        const url = 'https://www.starbucksthcampaign.com/campaign/bogo';

        await driver.get(url);
        const startButtonSelector = '.the-game a[onclick="playQuiz(event)"]';
        const startButton = await driver.findElement(By.css(startButtonSelector));

        await driver.wait(until.elementIsVisible(startButton), 15000);
        await driver.executeScript('arguments[0].scrollIntoView(true);', startButton);
        await startButton.click();

        const allQuestions = await getAllQuestions(driver);
        const selectedQuestions = await getSelectedQuestions(driver, allQuestions);

        for (let index = 0; index < selectedQuestions.length; index++) {
            await answerTheQuestion(driver);
        }

        const phoneNumberFields = await driver.findElements(By.css('#register input[type="tel"]'));
        await driver.wait(until.elementIsVisible(phoneNumberFields[0]), 15000);

        for (const phoneNumberField of phoneNumberFields) {
            await driver.executeScript('arguments[0].scrollIntoView(true);', phoneNumberField);
            await phoneNumberField.sendKeys(phoneNumber);
        }

        const submitButton = await driver.findElement(By.css('#register a'));
        await driver.executeScript('arguments[0].scrollIntoView(true);', submitButton);
        await submitButton.click();

        const sharePage = await driver.findElement(By.css('#share.pages'));
        await driver.wait(until.elementIsVisible(sharePage), 15000);
        await driver.sleep(5000);
    });

    afterEach(async () => {
        await driver.quit();
    })
});

async function getAllQuestions(driver: WebDriver): Promise<any> {
    const questions = await driver.executeScript('return window.campaign.questions;') as Question[];
    for (const question of questions) {
        let answer = question.choices.find((choice: Choice) => choice.acquire_score && choice.acquire_score == 1);
        question.answerId = answer ? answer.id : null;
    }
    return questions;
}

async function getSelectedQuestions(driver: WebDriver, allQuestions: Question[]): Promise<Question[]> {
    const selectedQuestionIds = await driver.executeScript('return window.allQuestions;') as number[];
    return allQuestions.filter(question => selectedQuestionIds.includes(question.id));
}

async function answerTheQuestion(driver: WebDriver): Promise<void> {
    const questionWrappers = await driver.findElements(By.css('.question-pages'));
    for (let index = 0; index < questionWrappers.length; index++) {
        const result = await driver.executeScript('return window.getComputedStyle(arguments[0]).display === "block";', questionWrappers[index]);
        if (result) {
            const choiceElements = await questionWrappers[index].findElements(By.css('.choices-list .db-adman-x-font'));
            await choiceElements[0].click();
            break;
        }
    }
}

function getPhoneNumber(): string {
    const commandLine = process.argv.join(' ');
    const regex = /--phone\s*(\d{10})/;
    const matches = regex.exec(commandLine);

    if (matches && matches.length > 1) {
        return matches[1];//group capture phone number
    } else {
        throw new Error(
            'Could not find your phone number from command line arguments, ' +
            'you need to give your phone number when run testing e.g., ' +
            'gulp test --phone 0812345678.'
        );
    }
}
