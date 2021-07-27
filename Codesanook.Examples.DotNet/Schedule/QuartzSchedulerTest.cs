using Microsoft.Data.Sqlite;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Codesanook.Examples.DotNet.Schedule
{
    public class QuartzSchedulerTest : IAsyncLifetime
    {
        private IScheduler scheduler;

        // https://stackoverflow.com/a/45906269/1872200
        public async Task InitializeAsync()
        {
            const string databaseName = "test.db";
            await CreateSQLiteDatabase(databaseName);
            // https://github.com/quartznet/quartznet/issues/517
            var properties = new NameValueCollection
            {
                { "quartz.serializer.type", "json" },
                // https://www.quartz-scheduler.net/documentation/quartz-3.x/tutorial/job-stores.html#ado-net-job-store-adojobstore
                { "quartz.jobStore.type", "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz" },
                { "quartz.jobStore.useProperties", "true" },
                { "quartz.jobStore.lockHandler.type", "Quartz.Impl.AdoJobStore.UpdateLockRowSemaphore, Quartz" },
                { "quartz.jobStore.driverDelegateType", "Quartz.Impl.AdoJobStore.SQLiteDelegate, Quartz" },
                { "quartz.jobStore.tablePrefix", "QRTZ_" },

                { "quartz.jobStore.dataSource", "default" },
                { "quartz.dataSource.default.provider", "SQLite-Microsoft" },
                { "quartz.dataSource.default.connectionString", $"Data Source={databaseName}" }
            };

            // Get a scheduler from a factory
            var factory = new StdSchedulerFactory(properties);
            scheduler = await factory.GetScheduler();

            // Start it
            await scheduler.Start();
        }

        public Task DisposeAsync() => Task.CompletedTask;

        [Fact]
        public async Task ScheduleJob_RunJobEveryFiveSecondsThreeTimes_JobRunsThreeTimes()
        {
            // Arrange
            // Define the job and tie it to our HelloJob class
            var job = JobBuilder.Create<HelloJob>()
                .WithIdentity(nameof(HelloJob))
                .Build();

            var utcNow = DateTime.UtcNow;
            var startTime = new DateTime(
                    utcNow.Year,
                    utcNow.Month,
                    utcNow.Day,
                    utcNow.Hour,
                    utcNow.Minute,
                    utcNow.Second,
                    utcNow.Kind
                );

            /*
            We want Cron job to run every 5 seconds.
            For example
            0s, 5s, 10s, 15s, ...  
            We start by getting the start time end with 0 or 5  
            Then we add 10 seconds which means we will end up having this Cron run 3 times 
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

            // Act
            // Tell Quartz to schedule the job using our trigger
            await scheduler.ScheduleJob(job, trigger);

            // Wait to get schedule finish
            await Task.Delay(TimeSpan.FromSeconds(15));
            await scheduler.Shutdown();

            // Assert
            var fireTimes = TriggerUtils.ComputeFireTimesBetween(
                (IOperableTrigger)trigger,
                null,
                startTime,
                endTime
            );
            Assert.Equal(3, fireTimes.Count);

            fireTimes.Select((fireTime, index) =>
                new { ExpectedFireTime = startTime.AddSeconds(5 * index), ActualFireTime = fireTime.UtcDateTime }
            ).ToList().ForEach(assertion =>
                Assert.Equal(assertion.ExpectedFireTime, assertion.ExpectedFireTime)
            );

            Assert.Equal(3, HelloJob.CounterValue);
        }

        private static async Task CreateSQLiteDatabase(string databaseName)
        {
            if (File.Exists(databaseName))
            {
                File.Delete(databaseName);
            }

            using var connection = new SqliteConnection($"Data Source={databaseName}");
            await connection.OpenAsync();
            var sql = await File.ReadAllTextAsync("Schedule/create-tables.sql");
            var command = new SqliteCommand(sql, connection);
            command.ExecuteNonQuery();
        }
    }
}
