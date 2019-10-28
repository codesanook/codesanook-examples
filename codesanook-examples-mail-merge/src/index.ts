declare let Logger: any;
declare let SpreadsheetApp: any;
declare let MailApp: any;

const EMAIL_SENT = 'EMAIL_SENT';
const sheetName = 'Sponsor Contacts Test'; // Change to your sheet name
const sheet = SpreadsheetApp.getActiveSpreadsheet().getSheetByName(sheetName);

/*
 * main function
 * Sends emails with data from the current spreadsheet.
*/
function sendEmails() {
    // Returns the position of the last row that has content.
    const lastRow = sheet.getLastRow();

    const dataRange = sheet.getRange(
        1, // The starting row index of the range; row indexing starts with 1.
        2, // The starting column index of the range; column indexing starts with 1.
        lastRow, // The number of rows to return.
        5, // The number of columns to return.
    );

    // Fetch values for each row in the Range.
    const data = dataRange.getValues() as any[];
    const header = data.shift() as any[];

    const mailItems: EmailRecord[] = data
        .filter((values: any[]) => values.every(value => value))// Remove empty rows
        .map((values, dataIndex) => {

            const emailRecord = header.reduce((obj, key, headerIndex) => {
                obj[key] = values[headerIndex];
                return obj as EmailRecord;
            }, {});

            emailRecord.dataIndex = dataIndex;
            emailRecord.subject = getSubject();
            emailRecord.body = getBody(emailRecord, getSenderProfile());
            emailRecord.toAddress = emailRecord.toAddress.trim();
            return emailRecord;
        });

    mailItems.forEach(item => {
        if (item.sentStatus === EMAIL_SENT) {
            return;
        }
        MailApp.sendEmail(
            item.toAddress,
            item.subject,
            item.body,
            { cc: item.ccAddresses, htmlBody: item.body },
        );
        const cell = sheet.getRange(item.dataIndex as number + 2, 1); // index range starts from 1
        cell.setValue(EMAIL_SENT);
    });
}

function getSenderProfile(): SenderProfile {
    const header = sheet.getRange(2, 8, 5, 1).getValues().map(value => value[0]);
    const data = sheet.getRange(2, 9, 5, 1).getValues() as any[];

    const senderProfile = header.reduce((obj, key, rowIndex) => {
        obj[key] = data[rowIndex][0];
        return obj as SenderProfile;
    }, {});

    return senderProfile;
}

function getSubject(): string {
    return 'Request for a list name of your staff who will attend .NET Conf 2019';
}

function getBody(emailRecord: EmailRecord, senderProfile: SenderProfile): string {

    const numberOfVIPGuest = emailRecord.packageType === 'gold' ? 5 : 3;

    const beginBody = [
        `Dear ${emailRecord.toName}`,
        ``,
        `Thanks again for supporting .NET Conf 2019. We appreciate it.`,
        `Since you are our ${emailRecord.packageType} package sponsor, we need this information from you.`,
        `- First name, last name and position of all ${emailRecord.companyName} staff who will attend .NET Conf on Oct 26 Oct, from 9 am to 6 pm.`,
        `- Please also specify ${numberOfVIPGuest} people who are VIP attendees because we would like to prepare a first row VIP chairs for them.`,
        ``,
    ];

    const goldPackageOnlyBody = [
        `In addition, we would like to inform you that:`,
        `- Please prepare decoration for your booth, stand banner and people who will take care of it.`,
        `- We provide you a table of size 2m x 3m, 2 chairs and an extension cord.`,
        `- Please also prepare 5 minutes presentation on the stage to show your company, products or anything that you want to communicate with the attendees who are developers.`,
        ``,
    ];

    const endBody = [
        `If you have any questions, please let me know.`,
        `We are looking forward to see you soon at Microsoft Thailand CRC tower All Season Place (https://goo.gl/maps/bw6SFjhAe9BCFruz5).`,
        ``,
        `Faithfully yours,`,
        `${senderProfile.name}`,
        ``,
        `--`,
        `${senderProfile.name}`,
        `${senderProfile.position}`,
        `${senderProfile.company}`,
        ``,
        `Tel: ${senderProfile.tel}`,
        `Email: ${senderProfile.email}`,
    ];

    const body = emailRecord.packageType === 'gold'
        ? [...beginBody, ...goldPackageOnlyBody, ...endBody]
        : [...beginBody, ...endBody];

    return body.join('<br/>');
}

interface EmailRecord {
    sentStatus: string;
    toName: string;
    toAddress: string;
    ccAddresses: string; // CSV
    companyName: string;
    packageType: string;
    subject?: string;
    body?: string;
    dataIndex?: number;
}

interface SenderProfile {
    name: string;
    position: string;
    company: string;
    tel: string;
    email: string;
}
