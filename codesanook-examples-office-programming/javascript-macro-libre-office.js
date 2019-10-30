// When this script is run on an existing spreadsheet,
// the value 2 will be placed in cell A1 of the first sheet
importClass(Packages.com.sun.star.uno.UnoRuntime);
importClass(Packages.com.sun.star.sheet.XSpreadsheetDocument);
importClass(Packages.com.sun.star.container.XIndexAccess);
importClass(Packages.com.sun.star.table.XCellRange);
importClass(Packages.com.sun.star.table.XCell);
importClass(Packages.com.sun.star.text.XText);

//get the document object from the scripting context
oDoc = XSCRIPTCONTEXT.getDocument();

//get the XSpreadsheetDocument interface from the document
xSDoc = UnoRuntime.queryInterface(XSpreadsheetDocument, oDoc);

//get the XIndexAccess interface used to access each sheet
xSheetsIndexAccess = UnoRuntime.queryInterface(XIndexAccess, xSDoc.getSheets());

//get sheet 0
xSheet = xSheetsIndexAccess.getByIndex(0);

//get the XCellRange interface used to access a cell
xCll = UnoRuntime.queryInterface(XCellRange, xSheet);

//get Cell A1
xCellA1 = xCll.getCellByPosition(0, 0);

//get the XCell interface used to access a cell
var xText = UnoRuntime.queryInterface(XText, xCellA1);
xText.setString("Hello world " + Date());