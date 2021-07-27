using System;
using NodaTime;
using Xunit;

namespace Codesanook.Examples.DotNet.Datetime
{
    public class TimeZoneTest
    {
        [Fact]
        public void ConvertUtcToLocalTime_ValidZoneId_ReturnCorrectLocalTime()
        {
            const string zoneId = "Asia/Bangkok";
            var timeZone = DateTimeZoneProviders.Tzdb[zoneId];

            var utcTime = new DateTime(2021, 1, 1, 7, 30, 0, DateTimeKind.Utc);
            var instant = Instant.FromDateTimeUtc(utcTime);
            var localTime = instant.InZone(timeZone).ToDateTimeUnspecified();

            var expectedLocalTime = new DateTime(2021, 1, 1, 14, 30, 0); // Bangkok offset is UTC+7
            Assert.Equal(expectedLocalTime, localTime);
        }
    }
}
