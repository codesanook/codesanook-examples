using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace Codesanook.Examples.DotNetCore.Office
{
    public class WordDocumentTest
    {

        [Fact]
        public void Test()
        {
            var document = new XWPFDocument();
            var p = document.CreateParagraph();
            var r0 = p.CreateRun();
            r0.SetText("FileFormat");
            r0.AddCarriageReturn();
            //r0.SetText("test");
            //r0.AddBreak(BreakClear.ALL);

            var table = document.CreateTable(rows: 5, cols: 2);
            // Row and cell start at index 0

            var col = table.GetRow(0).GetCell(0);
            var paragraph = col.AddParagraph();
            var run = paragraph.CreateRun();
            run.IsBold = true;
            run.FontFamily = "Courier";
            run.FontSize = 12;
            run.SetText("ชื่อและที่อยู่ผู้ส่ง");
            run.AddBreak();

            var run2 = paragraph.CreateRun();
            run2.IsBold = true;
            run2.FontFamily = "Courier";
            run2.FontSize = 12;
            run2.SetText("ชื่อและที่อยู่ผู้ส่ง");
            run2.AddBreak();

//            //run.SetText(
//@" 
//นายธีรานิตย์ พงค์ทองเมือง โทร 089 668 6365 
//เลขที่ 1055/1170  
//อาคาร State tower (RCK)
//ถ.สีลม แขวงสีลม
//เขตบางรัก กทม.

//10500"
//);
//            //run.SetUnderline(UnderlinePatterns.DotDotDash);
            //run.TextPosition = 100;

            var outputFile = new FileStream("table.docx", FileMode.Create);
            document.Write(outputFile);
            outputFile.Close();
        }
    }
}
