#tool "nuget:?package=nuget.commandline&version=5.3.0"

var configuration = Argument("Configuration", "Release");
var target = Argument("Target", "AfterBuildClean");

var outputDir = Directory($"../output/{configuration.ToLower()}");
var solution = "../CustomCapes.sln";

var execName = "./CustomCapes.exe";

var removeFilesWithExtension = new string[]{".pdb"};

Task("Install-Nuget")
    .Does(()=> {
    FilePath nugetPath = Context.Tools.Resolve("nuget.exe");
    StartProcess(nugetPath, new ProcessSettings {
        Arguments = new ProcessArgumentBuilder()
            .Append("help")
            .Append("install")
        });
});

Task("Clean")
    .Does(() =>
    {
        Information($"Cleaning output directory {outputDir}...");
        CleanDirectory(outputDir);
    });

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        Information($"Restoring nuget packages...");
        NuGetRestore(solution);
    });

    
Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
    {
    Information("Building from sources...");
    
    var settings = new MSBuildSettings{
        Configuration = configuration,
    };
    
    MSBuild(solution, settings);
    });

Task("AfterBuildClean")
    .IsDependentOn("Build")
    .Does(() => {
        
        Information("Cleaning files..."); 
        foreach(var extension in removeFilesWithExtension){
            var files = $"{outputDir}/*{extension}";
            Information($"\t{files}");
            DeleteFiles(GlobPattern.FromString(files));        
        }
        
    });

RunTarget(target);