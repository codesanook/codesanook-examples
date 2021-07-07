using Quartz;
using System.Threading.Tasks;

namespace Codesanook.Examples.DotNetCore.Schedule
{
    public class HelloJob : IJob
    {
        public static int CounterValue { get; private set; }

        public Task Execute(IJobExecutionContext context)
        {
            CounterValue++;
            return Task.CompletedTask;
        }
    }
}
