declare let Logger: any;
declare let SpreadsheetApp: any;
declare let MailApp: any;

const EMAIL_SENT = 'EMAIL_SENT';

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
    rowIndex?: number;
}

interface SenderProfile {
    name: string;
    position: string;
    company: string;
    tel: string;
    email: string;
}

/**
 * Sends emails with data from the current spreadsheet.
 */
function sendEmails() {
    const sheet = SpreadsheetApp.getActiveSheet();

    // Fetch the range of cells A2:B3
    const dataRange = sheet.getRange('A2:F3');
    // Fetch values for each row in the Range.
    const data = dataRange.getValues() as any[];

    const mailItems = data.map((row, index) => {
        const emailRecord: EmailRecord = {
            sentStatus: '',
            toName: '',
            toAddress: '',
            ccAddresses: '', // CSV
            companyName: '',
            packageType: '',
        };

        emailRecord.rowIndex = index;

        Object.keys(emailRecord).map((propertyName, propertyIndex) => {
            const cellValue = row[propertyIndex];
            if (cellValue) {
                emailRecord[propertyName] = cellValue;
            }
        });

        emailRecord.subject = getSubject();
        emailRecord.body = getBody(emailRecord, getSenderProfile());
        return emailRecord;
    });

    mailItems.forEach(item => {
        if (item.sentStatus === EMAIL_SENT) {
            return;
        }

        MailApp.sendEmail(item.toAddress, item.subject, item.body, { cc: item.ccAddresses, htmlBody: item.body });
        const cell = sheet.getRange(item.rowIndex as number + 2, 1); // range is start from 1
        cell.setValue(EMAIL_SENT);
    });
}

function getSenderProfile(): SenderProfile {
    const senderProfile: SenderProfile = {
        name: '',
        position: '',
        company: '',
        tel: '',
        email: '',
    };

    const sheet = SpreadsheetApp.getActiveSheet();

    const dataRange = sheet.getRange('I2:I6');
    const data = dataRange.getValues() as any[];
    Object.keys(senderProfile).forEach((propertyName, index) => {
        senderProfile[propertyName] = data[index][0];
    });
    return senderProfile;
}
