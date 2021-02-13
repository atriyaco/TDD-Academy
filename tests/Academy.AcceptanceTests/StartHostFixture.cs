using System;
using Academy.AcceptanceTests.Core;
using Academy.AcceptanceTests.NetCoreHosting;

namespace Academy.AcceptanceTests
{
    public class StartHostFixture : IDisposable
    {
        private DotNetCoreHost _host = new DotNetCoreHost(new DotNetCoreHostOptions
        {
            Port = HostConstants.Port,
            CsProjectPath = HostConstants.CsProjectPath
        });

        public StartHostFixture()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var executablePath = HostConstants.CsProjectPath;

            if (environment == "Staging")
            {
                executablePath =
                    "/home/runner/work/TDD-Academy/TDD-Academy/source/Academy.Presentation/Academy.Presentation.csproj";
            }

            Console.WriteLine("*************************");
            Console.WriteLine(environment);
            Console.WriteLine("*************************");

            _host._options.CsProjectPath = executablePath;
            _host.Start();
        }

        public void Dispose()
        {
            _host.Stop();
        }
    }
}