using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Codesanook.Examples.DotNetCore.AsyncProgramming
{
    public class AsyncVoidTest
    {
        private readonly ITestOutputHelper output;
        public AsyncVoidTest(ITestOutputHelper output) => this.output = output;

        [Fact]
        public void AsyncVoid_DoFireAndForget_CallerCannotCatchException()
        {
            // Not a native way to assert exception in xUnit, just to clarify Fire and forget 
            // For production code, use Assert.Throws<ArgumentException>(() => instead
            Exception exceptionFromSimulatedOperation = null; ;
            try
            {
                output.WriteLine($"Before calling SimulatedOperation at {DateTime.Now}");
                SimulateOperation();
                output.WriteLine($"After SimulatedOperation get called at {DateTime.Now}");
            }
            catch (Exception ex)
            {
                exceptionFromSimulatedOperation = ex;
            }

            Assert.Null(exceptionFromSimulatedOperation);
        }

        private async void SimulateOperation()
        {
            await Task.Delay(2000);
            throw new InvalidOperationException();
        }
    }
}
