using System;
using System.Globalization;
using System.Threading;
using Xunit;

namespace Codesanook.Examples.DotNet.Localization
{
    public class LocalizationTest : IDisposable
    {
        private readonly string originalCulture;
        public LocalizationTest() => originalCulture = Thread.CurrentThread.CurrentCulture.Name;
        public void Dispose() => Thread.CurrentThread.CurrentCulture = new CultureInfo(originalCulture);

        [Theory]
        [InlineData("en-US", -1)]
        [InlineData("th-TH", 0)]
        public void IndexOfToSearchNonExistingString_UseCurrentCulture_ReturnStringAtIndex(string CurrentCulture, int expectedFoundStringAtIndex)
        {
            // Arranage 
            Thread.CurrentThread.CurrentCulture = new CultureInfo(CurrentCulture);
            const string inputValue = "about-us";

            // Act
            var actualFoundStringAtIndex = inputValue.IndexOf("//");

            // Assert
            Assert.Equal(expectedFoundStringAtIndex, actualFoundStringAtIndex);
        }
    }
}
