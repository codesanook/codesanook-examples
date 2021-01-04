// Revenue summary script by Codesanook Team
// Credit Wittawat Karpkrikaew 2

// Triple-Slash Directives
// https://www.typescriptlang.org/docs/handbook/triple-slash-directives.html
/// <reference path='../../node_modules/@types/google-apps-script/google-apps-script.script.d.ts' />
const spreadSheet = SpreadsheetApp.getActive();

function toFloat(inputNumber: number) {
  // From https://www.saintsatplay.com/blog/2014-08-02-handling-floating-point-numbers-in-javascript
  return Math.round(parseFloat((inputNumber * Math.pow(10, 2)).toFixed(2))) / Math.pow(10, 2);
}

function getSummaryForCurrency(currentcy: string) {
  const textFinder = spreadSheet.createTextFinder(currentcy);
  // const revenues = textFinder.findAll().map(r => parseFloat(r.getValue().replace(/[\$\s,]+/g, '')));

  const revenues = textFinder.findAll().map(r => toFloat(r.getValue()));
  const totalRevenues = revenues.reduce((sum, currentValue) => {
    sum += currentValue;
    return sum;
  }, 0.0);

  Logger.log(`total for ${currentcy}: ${totalRevenues.toFixed(2)}`);
}

function getTotalSummary() {
  const sheets = spreadSheet.getSheets();
  Logger.log(`total records ${sheets.length}`);
  getSummaryForCurrency('$');
  getSummaryForCurrency('€');
}
