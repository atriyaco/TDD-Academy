namespace Academy.AcceptanceTests.Core
{
    public interface IStartableHost : IHost
    {
        void Start();
        void Stop();
    }
}