var target = Argument("target", "Default");
var configuration = Argument("configuration", "Debug");

void BuildProject(string projectPath, string configuration)
{
    var settings = new DotNetCoreBuildSettings();
    settings.SetConfiguration(configuration);

    // settings =>
    //     settings.SetConfiguration(configuration)
    //     .SetVerbosity(Verbosity.Minimal)
    //     .WithTarget("Build")
    //     .WithProperty("NodeReuse", "false")
	// 	.WithProperty("Platform", "AnyCPU")
    DotNetCoreBuild(projectPath, settings);
}

Task("Default")
    .IsDependentOn("Build")    
    .Does(() => {
  
    });

Task("RestorePackages")
    .Does(() => {
        DotNetCoreRestore("src/DlnaCore.Server/");
    });

Task("Build")
    .IsDependentOn("RestorePackages")
    .Does(() => {
        BuildProject("src/DlnaCore.Server/DlnaCore.csproj",configuration);
    });

RunTarget(target);