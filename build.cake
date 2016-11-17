// Arguments
var target = Argument<string>("target", "Default");

// Variables
var artifactsDir = Directory("artifacts");

Task("Default")
    .IsDependentOn("Compile");

Task("Clean")
    .Does(() =>
    {
        CleanDirectories(new DirectoryPath[] { artifactsDir });
    });

Task("Restore-NuGet-Packages")
    .Description("Restores NuGet packages")
    .Does(() =>
    {
        var settings = new DotNetCoreRestoreSettings
        {
            Verbose = false,
            PackagesDirectory = "./src/packages",
            Sources = new [] { 
                "https://api.nuget.org/v3/index.json"
            },
        };
        DotNetCoreRestore("./src/LibLog2/LibLog2.csproj", settings);

        NuGetRestore("./src/LibLog.sln");
    });

Task("Compile")
    .Description("Builds the solution")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
    {
        var settings = new MSBuildSettings{
            Verbosity = Verbosity.Minimal,
            ToolVersion = MSBuildToolVersion.VS2015,
            Configuration = "Release",
            PlatformTarget = PlatformTarget.MSIL
        };
        MSBuild("./src/LibLog.sln", settings);
    });

RunTarget(target);