using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using NLog.Fluent;
using System.Reflection.Metadata;

namespace Codesanook.Examples.CSharp.Tests.Logging
{
    class NLogTest
    {

        public void Test()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            var provider = services.BuildServiceProvider();

            var factory = provider.GetService<ILoggerFactory>();
            factory.AddNLog();
            factory.ConfigureNLog("nlog.config");

            var logger = provider.GetService<ILogger<Log4NetTest>>();
            logger.LogCritical("hello nlog");
            //<PackageReference Include="NLog.Extensions.Logging" Version="1.0.0-rtm-beta5" />
            //<PackageReference Include="Microsoft.Extensions.Configuration" Version="1.1.2" />
            //<PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="1.1.1" />
            //<PackageReference Include="Microsoft.Extensions.Logging" Version="1.1.2" />
        }
    }
}
