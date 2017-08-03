var target = Argument("target", "Default");
var configuration = Argument("configuration", "Debug");

void BuildProject(string projectPath, string configuration)
{
    var settings = new DotNetCoreBuildSettings();
    settings.Configuration = configuration;

    DotNetCoreBuild(projectPath, settings);
}

Task("Default")
    .IsDependentOn("RunTests")    
    .Does(() => {
  
    });

Task("RestorePackages")
    .Does(() => {
        DotNetCoreRestore("src/DlnaCore.Server/");
    });

Task("RestorePackagesTests")
    .Does(() => {
        DotNetCoreRestore("tests/DlnaCore.Tests/");
    });

Task("Build")
    .IsDependentOn("RestorePackages")
    .Does(() => {
        BuildProject("src/DlnaCore.Server/DlnaCore.csproj",configuration);
    });

Task("BuildTests")
    .IsDependentOn("RestorePackagesTests")
    .Does(() => {
        BuildProject("tests/DlnaCore.Tests/DlnaCore.Tests.csproj",configuration);
    });

Task("RunTests")
    .IsDependentOn("Build")
    .IsDependentOn("BuildTests")
    .Does(() => {
        DotNetCoreTest("tests/DlnaCore.Tests/DlnaCore.Tests.csproj");
    });


RunTarget(target);