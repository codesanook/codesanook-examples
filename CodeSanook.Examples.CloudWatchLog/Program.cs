using Amazon.Runtime;
using AWS.Logger.Log4net;
using log4net;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.Linq;

namespace CodeSanook.Examples.CloudWatchLog
{
    public class Program
    {
        private static readonly ILog log = LogManager.GetLogger(nameof(Program));
        private static string awsApiKey;
        private static string awsScretKey;

        public static void Main(string[] args)
        {
            SetAwsKey(args);

            var appender = CreateCloudWatchLogAppender();
            var hierarchy = (Hierarchy)log.Logger.Repository;
            var logger = hierarchy.GetLogger(nameof(Program)) as Logger;
            logger.AddAppender(appender);

            //Start writing log
            log.Debug("Hello world logging.");
            log.Info("Hello world logging.");
            log.Error("Hello world logging.");
        }

        private static void SetAwsKey(string[] args)
        {
            if(args ==null || !args.Any())
            {
                throw new ArgumentNullException(
                    "args", 
                    "Please pass AWS secret key and API as command line arguments"
                );
            }

            //TODO this is only simple example, not for a production code
            awsApiKey = args[0];
            awsScretKey = args[1];
            //Output values to verify that we have pass a values from command line arguments
            Console.WriteLine(
                $"{nameof(awsApiKey)}: {awsApiKey.Substring(0, 8)}, " +
                $"{nameof(awsScretKey)}: {awsScretKey.Substring(0, 8)}"
            );
        }

        private static AWSAppender CreateCloudWatchLogAppender()
        {
            var patternLayout = new PatternLayout
            {
                ConversionPattern = "%utcdate{yyyy-MM-ddTHH:mm:ss.fffZ} [%-5level] %logger - %message%newline"
            };
            patternLayout.ActivateOptions();

            var appender = new AWSAppender
            {
                Layout = patternLayout,
                Credentials = new BasicAWSCredentials(awsApiKey, awsScretKey),
                LogGroup = "CodeSanook.Examples.CloudWatchLog",
                Region = "ap-southeast-1"
            };

            appender.ActivateOptions();
            return appender;
        }
    }
}