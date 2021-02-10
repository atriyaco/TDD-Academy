using System;
using Academy.AcceptanceTests.Core;
using Academy.AcceptanceTests.NetCoreHosting;

namespace Academy.AcceptanceTests
{
    public class StartHostFixture : IDisposable
    {
        private readonly IStartableHost _host = new DotNetCoreHost(new DotNetCoreHostOptions
        {
            Port = HostConstants.Port,
            CsProjectPath = HostConstants.CsProjectPath
        });

        public StartHostFixture()
        {
            _host.Start();
        }

        public void Dispose()
        {
            _host.Stop();
        }
    }
}