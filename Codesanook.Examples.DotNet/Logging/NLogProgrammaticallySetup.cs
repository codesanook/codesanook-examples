using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Codesanook.Examples.DotNetCore.Tests.Logging
{
    class NLogProgrammaticallySetup
    {
        public void Test()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            var provider = services.BuildServiceProvider();

            var factory = provider.GetService<ILoggerFactory>();
            factory.AddNLog();
            factory.ConfigureNLog("nlog.config");

            var logger = provider.GetService<ILogger<NLogProgrammaticallySetup>>();
            logger.LogCritical("hello nlog");
            //<PackageReference Include="NLog.Extensions.Logging" Version="1.0.0-rtm-beta5" />
            //<PackageReference Include="Microsoft.Extensions.Configuration" Version="1.1.2" />
            //<PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="1.1.1" />
            //<PackageReference Include="Microsoft.Extensions.Logging" Version="1.1.2" />
        }
    }
}
