using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace Codesanook.Examples.DotNet.AsyncProgramming
{
    public class ThreadPoolTest
    {
        [Fact]
        public void Test()
        {

            int minWorker, minIOC;
            // Get the current settings.
            ThreadPool.GetMinThreads(out minWorker, out minIOC);

            int maxWorker, maxIOC;
            ThreadPool.GetMaxThreads(out maxWorker, out maxIOC);
            // Change the minimum number of worker threads to four, but
            // keep the old setting for minimum asynchronous I/O 
            // completion threads.
            var result = ThreadPool.SetMaxThreads(short.MaxValue + 10,  maxIOC);
            if (ThreadPool.SetMinThreads(4, minIOC))
            {
                // The minimum number of threads was set successfully.
            }
            else
            {
                // The minimum number of threads was not changed.
            }

            ThreadPool.GetMinThreads(out minWorker, out minIOC);
        }
    }
}
