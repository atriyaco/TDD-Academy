using Academy.AcceptanceTests;
using Academy.AcceptanceTests.NetCoreHosting;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[ShutdownDotNetAfterServerBuild]
public class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.E2ETests);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;

    AbsolutePath SourceDirectory => RootDirectory / "source";
    AbsolutePath TestsDirectory => RootDirectory / "tests";
    AbsolutePath OutputDirectory => RootDirectory / "output";

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(OutputDirectory);
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoRestore());
        });

    Target UnitTest => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            RunTest("Academy.Domain.Tests.Unit");
            RunTest("Academy.Application.Tests.Unit");
            RunTest("Academy.Presentation.Tests.Unit");
            RunTest("Academy.Infrastructure.Tests.Unit");
        });

    Target E2ETests => _ => _
        .DependsOn(UnitTest)
        .Executes(() =>
        {
            var host = StartUpHost();
            RunTest("Academy.AcceptanceTests");
            StopHost(host);
        });

    void RunTest(string projectName)
    {
        var project = Solution.GetProject(projectName).NotNull();
        DotNetTest(x => x.SetProjectFile(project));
    }

    public DotNetCoreHost StartUpHost()
    {
        var host = new DotNetCoreHost(new DotNetCoreHostOptions()
        {
            Port = HostConstants.Port,
            CsProjectPath = HostConstants.CsProjectPath
        });
        host.Start();
        return host;
    }

    public void StopHost(DotNetCoreHost host)
    {
        host.Stop();
    }
}