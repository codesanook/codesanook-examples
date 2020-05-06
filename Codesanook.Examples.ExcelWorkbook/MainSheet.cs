using System;
using System.Data;
using System.Linq;

namespace Codesanook.Examples.ExcelWorkbook
{
    public partial class MainSheet
    {
        private void MainSheet_Startup(object sender, EventArgs e)
        {
            const int year = 2020;

            var orderedDayOfWeeks = new[]
            {
                DayOfWeek.Monday,
                DayOfWeek.Tuesday,
                DayOfWeek.Wednesday,
                DayOfWeek.Thursday,
                DayOfWeek.Friday,
                DayOfWeek.Saturday,
                DayOfWeek.Sunday
            };

            var firstDateOfYear = new DateTime(year, 1, 1);
            var firstMondayOfFirstWeek = firstDateOfYear.AddDays(
                -Array.IndexOf(orderedDayOfWeeks, firstDateOfYear.DayOfWeek)
            );

            var weekRanges =
                from date in Enumerable.Range(0, int.MaxValue)
                let mondayOfWeek = firstMondayOfFirstWeek.AddDays(date * 7) // Create a next Monday
                select (From: mondayOfWeek, To: mondayOfWeek.AddDays(4));

            var weekRangesInYear = weekRanges.TakeWhile(x => x.From.Year <= year).ToArray();
            var firstRowNumber = 5;

            for (int index = 0; index < weekRangesInYear.Length; index++)
            {
                var range = get_Range($"A{ firstRowNumber + index}");
                var formattedDate = 
                    $"# {index + 1:0#}) {weekRangesInYear[index].From:MMM dd} - {weekRangesInYear[index].To:MMM dd}";

                // Value2 works the same way as Range.Value, except that it does not check the cell format and convert to Date or Currency. 
                // And that's probably why its faster than .Value when retrieving numbers.
                // So .Value2 really should be the default, and is definitely the one to use 99% of the time.
                // From https://fastexcel.wordpress.com/2011/11/30/text-vs-value-vs-value2-slow-text-and-how-to-avoid-it/
                range.Value2 = formattedDate;
            }
        }

        #region VSTO Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            Startup += new EventHandler(MainSheet_Startup);
        }

        #endregion
    }
}
