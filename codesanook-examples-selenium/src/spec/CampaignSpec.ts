import { Builder, By, Capabilities, until, WebDriver, WebElement } from 'selenium-webdriver';
import * as chrome from 'selenium-webdriver/chrome';
import Question, { Choice } from '../models/Question';
const path: string = require('chromedriver').path;

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
                    //.setMobileEmulation({ deviceName: "iPhone X" })
                    .addArguments("--incognito")
            )
            .build();
    });

    it("should able to submit correct answers with a phone number", async () => {
        let phoneNumber = getPhoneNumber();

        await driver.get("https://starbucksthcampaign.com/c/quiz_2018_summer_3");
        let startButtonSelector = ".the-game img[alt='Start Game']";

        let startButton = await driver.findElement(By.css(startButtonSelector));
        await driver.wait(until.elementIsVisible(startButton), 15000);

        let questionAnswerPairs = await getQuestionAnswerPairs(driver);
        await driver.executeScript("arguments[0].scrollIntoView(true);", startButton);
        await startButton.click();

        for (let questionNumber = 1; questionNumber <= 3; questionNumber++) {
            await answerTheQuestion(driver, questionAnswerPairs, questionNumber);
        }

        let phoneNumberFields = await driver.findElements(By.css("#user-profile-field input[type='tel']"));
        for (const phoneNumberField of phoneNumberFields) {
            await driver.executeScript("arguments[0].scrollIntoView(true);", phoneNumberField);
            await phoneNumberField.sendKeys(phoneNumber);
        }

        let submitButton = await driver.findElement(By.css("#user-profile-field a"));
        await driver.executeScript("arguments[0].scrollIntoView(true);", submitButton);
        await submitButton.click();

        let sharePage = await driver.findElement(By.css("#share.the-game"));
        await driver.wait(until.elementIsVisible(sharePage), 15000);
    });

    afterEach(async () => {
        await driver.quit();
    })
});

async function getQuestionAnswerPairs(driver: WebDriver): Promise<any> {
    let questions = await driver.executeScript("return window.questions;") as Question[];
    let questionAnswerPairs: any = {};
    for (const question of questions) {
        let answer = question.choices.find((choice: Choice) => choice.acquire_score == 1);
        questionAnswerPairs[question.id] = answer.id;
    }
    return questionAnswerPairs;
}

async function answerTheQuestion(driver: WebDriver, questionAnswerPairs: any, questionNumber: number): Promise<void> {
    let title = await driver.findElement(By.css(`#campaign-pages-${questionNumber} .question-title`));
    await driver.wait(until.elementIsVisible(title), 10000);
    await driver.executeScript("arguments[0].scrollIntoView(true);", title);
    let titleText = await title.getText();
    let questionId = await getCurrentQuestionId(title);
    let choices = await driver.findElements(By.css(`#campaign-pages-${questionNumber} .choices-list .db-adman-x-font`));
    let choiceIdElementPairs: any = {};

    //https://stackoverflow.com/a/37576787/1872200
    for (const choice of choices) {
        const choiceId = await choice.findElement(By.css("input[type='radio']")).getAttribute("value");
        choiceIdElementPairs[choiceId] = choice;
    }
    let answerId = questionAnswerPairs[questionId];
    await choiceIdElementPairs[answerId].click();
}

async function getCurrentQuestionId(title: WebElement): Promise<number> {
    //match text ends with number and group capture number that will have index 1
    const questionIdPattern: RegExp = /[^\d]*(\d+)$/i;
    let questionIdValue = await title.getAttribute("id");
    let matches = questionIdPattern.exec(questionIdValue);
    let questionId: number = Number.parseInt(matches[1]);
    return questionId;
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