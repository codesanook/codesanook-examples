using System;
using System.Threading.Tasks;
using Xunit;

namespace Codesanook.Examples.DotNetCore.AsyncProgramming
{
    public class FlattenAggregateException
    {
        [Fact]
        public void Test()
        {
            var exception1 = new Exception("handle exception in task1"); ;
            var task1 = Task.Run(() =>
            {
                throw exception1;
            });

            var exception2 = new Exception("handle exception in task2"); ;
            var task2 = Task.Run(() =>
            {
                throw exception2;
            });

            var exception3 = new Exception("unhandle exception in task3");
            var task3 = Task.Run(() =>
            {
                Task.Factory.StartNew(() => throw exception3, TaskCreationOptions.AttachedToParent);
            });

            try
            {
                Task.WaitAll(task3, task1, task2);
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException;
                var inners = ((AggregateException)ex).Flatten().InnerExceptions;

                Assert.Equal(
                    new[] { exception1, exception2 },
                    inners
                );
            }
        }
    }
}
