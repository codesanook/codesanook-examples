using NodaTime;
using Xunit;
using Xunit.Abstractions;

namespace Codesanook.Examples.DotNetCore.Datetime
{
    public class DatePeriodDiffTest
    {
        private readonly ITestOutputHelper output;
        public DatePeriodDiffTest(ITestOutputHelper output) => this.output = output;

        [Fact]
        public void PeriodBetween_NoUnit_ReturnAllYearsMonthsDaysDiff()
        {
            var begin = new LocalDate(2020, 3, 16);
            var end = new LocalDate(2021, 3, 17);
            var period = Period.Between(begin, end);
            LogDiff(period);

            Assert.Equal(1, period.Years);
            Assert.Equal(0, period.Months);
            Assert.Equal(1, period.Days);
        }

        [Fact]
        public void PeriodBetween_MonthsUnit_ReturnOnlyMonthsDiff()
        {
            var begin = new LocalDate(2020, 3, 16);
            var end = new LocalDate(2021, 3, 17);
            var period = Period.Between(begin, end, PeriodUnits.Months);
            LogDiff(period);

            Assert.Equal(0, period.Years);
            Assert.Equal(12, period.Months);
            Assert.Equal(0, period.Days);
        }

        [Fact]
        public void PeriodBetween_DaysUnit_ReturnOnlyDaysDiff()
        {
            var begin = new LocalDate(2020, 3, 16);
            var end = new LocalDate(2021, 3, 17);
            var period = Period.Between(begin, end, PeriodUnits.Days);
            LogDiff(period);

            Assert.Equal(0, period.Years);
            Assert.Equal(0, period.Months);
            Assert.Equal(366, period.Days);
        }

        // FYI, you don't need to log this information just for demo purpose
        private void LogDiff(Period period) =>
            output.WriteLine($"years diff: {period.Years}, months diff: {period.Months}, days diff: {period.Days}");
    }
}
