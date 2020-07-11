using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Xunit;

namespace Codesanook.Examples.DotNetCore.LibraryExamples
{
    public class QuartzSchedulerTest
    {
        [Fact]
        public async Task Test()
        {
            // Grab the Scheduler instance from the Factory
            var props = new NameValueCollection
            {
                { "quartz.serializer.type", "binary" }
            };

            var factory = new StdSchedulerFactory(props);
            var scheduler = await factory.GetScheduler();

            // start it
            await scheduler.Start();

            // define the job and tie it to our HelloJob class
            var job = JobBuilder.Create<HelloJob>()
                .WithIdentity(nameof(HelloJob))
                .Build();

            var utcNow = DateTime.UtcNow;
            var startTime = new DateTime(
                    utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour, utcNow.Minute, utcNow.Second, utcNow.Kind
                );

            /*
            We want Cron job run every 5 seconds.
            For example
            0s, 5s, 10s, 15s, ...  
            We start by getting the start time end with 0 or 5  
            Then we add 10 seconds which means we will endup having this Cron run at 3 times 
            */
            // Remove not divided by five seconds and add 5 seconds
            startTime = startTime.AddSeconds(-(startTime.Second % 5) + 5);
            // Add 10 seconds for next two times
            var endTime = startTime.AddSeconds(10);

            // Trigger the job and that run every 5 seconds at start at a 0 second of a minute
            var trigger = TriggerBuilder.Create()
                .WithIdentity("cronTrigger")
                .WithCronSchedule(
                    "0/5 * * * * ? *",
                    cron => cron.InTimeZone(TimeZoneInfo.Utc)
                )
                .StartAt(startTime)
                .EndAt(endTime)
                .Build();

            // Tell quartz to schedule the job using our trigger
            await scheduler.ScheduleJob(job, trigger);

            // Some sleep to get schedules exected
            await Task.Delay(TimeSpan.FromSeconds(15));
            await scheduler.Shutdown();

            var times = TriggerUtils.ComputeFireTimesBetween(
                (IOperableTrigger)trigger,
                null,
                startTime,
                endTime
            );

            Assert.Equal(3, times.Count);
            for (int index = 0; index < times.Count; index++)
            {
                Assert.Equal(startTime.AddSeconds(5 * index), times[index].UtcDateTime);
            }

            Assert.Equal(3, HelloJob.CounterValue);
        }
    }

    public class HelloJob : IJob
    {
        public static int CounterValue;

        public Task Execute(IJobExecutionContext context)
        {
            CounterValue++;
            return Task.CompletedTask;
        }
    }
}
