using System;
using System.Diagnostics;
using System.Threading;
using Academy.AcceptanceTests.Core;

namespace Academy.AcceptanceTests.NetCoreHosting
{
    public class DotNetCoreHost : IStartableHost
    {
        private readonly DotNetCoreHostOptions _options;
        public string BaseUrl => $"http://localhost:{_options.Port}";
        private readonly AutoResetEvent _resetEvent = new AutoResetEvent(false);

        public DotNetCoreHost(DotNetCoreHostOptions options)
        {
            _options = options;
        }
        
        public void Start()
        {
            ProcessManager.KillByPort(_options.Port);
            var startInfo = new ProcessStartInfo("dotnet")
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                Arguments = $"run --project \"{_options.CsProjectPath}\"",
            };
            var process = Process.Start(startInfo);

            process.ErrorDataReceived += ProcessOnErrorDataReceived;
            process.OutputDataReceived += ProcessOnOutputDataReceived;

            process.BeginErrorReadLine();
            process.BeginOutputReadLine();

            _resetEvent.WaitOne();
        }

        private void ProcessOnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null && e.Data.Contains("Now listening on", StringComparison.OrdinalIgnoreCase))
                _resetEvent.Set();
        }

        private static void ProcessOnErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
                throw new Exception(e.Data);
        }

        public void Stop()
        {
            ProcessManager.KillByPort(_options.Port);
        }
    }
}