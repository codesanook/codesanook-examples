import { Builder, By, Capabilities, until, WebDriver, WebElement } from 'selenium-webdriver';
import * as chrome from 'selenium-webdriver/chrome';
import Question, { Choice } from '../models/Question';
const path: string = require('chromedriver').path;

describe("open starbucksthcampaign.com", () => {

    let driver: WebDriver = null;
    beforeEach(async () => {
        //Override the default value which is 5000 milliseconds
        //https://jasmine.github.io/2.1/introduction.html#section-Asynchronous_Support
        jasmine.DEFAULT_TIMEOUT_INTERVAL = 30000;

        //create service
        let service = new chrome.ServiceBuilder(path);
        driver = await new Builder()
            .withCapabilities(Capabilities.chrome())
            .setChromeService(service)
            .setChromeOptions(
                new chrome.Options()
                    //.setMobileEmulation({ deviceName: "iPhone X" })
                    .addArguments("--incognito")
            )
            .build();
    });

    it("should able to submit correct answers with a phone number", async () => {
        let phoneNumber = getPhoneNumber();
        const url = "https://www.starbucksthcampaign.com/campaign/bogo";

        await driver.get(url);
        let startButtonSelector = ".the-game a[onclick='playQuiz(event)']";
        let startButton = await driver.findElement(By.css(startButtonSelector));
        await driver.wait(until.elementIsVisible(startButton), 15000);
        await driver.executeScript("arguments[0].scrollIntoView(true);", startButton);
        await startButton.click();

        let allQuestions = await getAllQuestions(driver);
        let selectedQuestions = await getSelectedQuestions(driver, allQuestions);

        for (let index = 0; index < selectedQuestions.length; index++) {
            await answerTheQuestion(driver);
        }

        let phoneNumberFields = await driver.findElements(By.css("#register input[type='tel']"));
        await driver.wait(until.elementIsVisible(phoneNumberFields[0]), 15000);

        for (const phoneNumberField of phoneNumberFields) {
            await driver.executeScript("arguments[0].scrollIntoView(true);", phoneNumberField);
            await phoneNumberField.sendKeys(phoneNumber);
        }

        let submitButton = await driver.findElement(By.css("#register a"));
        await driver.executeScript("arguments[0].scrollIntoView(true);", submitButton);
        await submitButton.click();

        let sharePage = await driver.findElement(By.css("#share.pages"));
        await driver.wait(until.elementIsVisible(sharePage), 15000);
        await driver.sleep(5000);
    });

    afterEach(async () => {
        await driver.quit();
    })
});

async function getAllQuestions(driver: WebDriver): Promise<any> {
    let questions = await driver.executeScript("return window.campaign.questions;") as Question[];
    for (const question of questions) {
        let answer = question.choices.find((choice: Choice) => choice.acquire_score && choice.acquire_score == 1);
        question.answerId = answer ? answer.id : null;
    }
    return questions;
}

async function getSelectedQuestions(driver: WebDriver, allQuestions: Question[]): Promise<Question[]> {
    let selectedQuestionIds = await driver.executeScript("return window.allQuestions;") as number[];
    return allQuestions.filter(question => selectedQuestionIds.includes(question.id));
}

async function answerTheQuestion(driver: WebDriver): Promise<void> {
    let questionWrappers = await driver.findElements(By.css(".question-pages"));
    for (let index = 0; index < questionWrappers.length; index++) {
        let result = await driver.executeScript("return window.getComputedStyle(arguments[0]).display === 'block';", questionWrappers[index]);
        if (result) {
            let choiceElements = await questionWrappers[index].findElements(By.css(".choices-list .db-adman-x-font"));
            await choiceElements[0].click();
            break;
        }
    }
}

function getPhoneNumber(): string {
    let commandLine = process.argv.join(" ");
    const regex = /--phone\s*(\d{10})/;
    let matches = regex.exec(commandLine);

    if (matches && matches.length > 1) {
        return matches[1];//group capture phone number
    } else {
        throw new Error(
            "Could not find your phone number from command line arguments, " +
            "you need to give your phone number when run testing e.g., " +
            "gulp test --phone 0812345678."
        );
    }
}