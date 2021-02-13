using Academy.AcceptanceTests.NetCoreHosting;

namespace Academy.AcceptanceTests.Core
{
    public interface IStartableHost : IHost
    {
        void Start();
        void Stop();
    }
}