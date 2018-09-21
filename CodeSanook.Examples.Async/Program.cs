using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSanook.Examples.Async
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var task1 = Task.Run(() =>
            {
                throw new Exception("handle exception in task1");
            });

            var task2 = Task.Run(() =>
            {
                throw new Exception("handle exception in task2");
            });

            var task3 = Task.Run(() =>
            {
                Task.Factory.StartNew(() => throw new Exception("inner unhandle exception inside 3"), TaskCreationOptions.AttachedToParent);
                throw new Exception("unhandle exception in task3");
            });

            try
            {
              Task.WaitAll(task3, task1, task2);
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException;
                var inners = ((AggregateException)ex).Flatten().InnerExceptions;

            }
        }
    }
}
