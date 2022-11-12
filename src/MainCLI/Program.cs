namespace Fraxiinus.LeagueBackupManager.MainCLI;

using Fraxiinus.LeagueBackupManager.LeagueVersionControl;
using Fraxiinus.LeagueBackupManager.MainCLI.Helpers;
using System.CommandLine;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        return await GetCommandTree().InvokeAsync(args);
    }

    private static RootCommand GetCommandTree()
    {
        return new RootCommand()
        {
            new Command("list", "List available patches in repository").WithHandler(nameof(HandleListCommand)),
            new Command("last", "Show last exported patch").WithHandler(nameof(HandleLastCommand)),
            new Command("import", "Import patch into repository")
            {
                new Argument<string>("path", "Folder containing League of Legends.exe")
            }.WithHandler(nameof(HandleImportCommand)), 
            new Command("export", "Export patch from repository")
            {
                new Argument<string>("version", "Version of patch to export")
            }.WithHandler(nameof(HandleExportCommand)),
            new Command("delete", "Delete patch from repository")
            {
                new Argument<string>("version", "Version of patch to delete")
            }
        };
    }

    private static async Task HandleListCommand()
    {
        var repository = await LoadRepository();
        var availablePatches = repository.GetAvailablePatches();
        foreach (var patch in availablePatches)
        {
            Console.WriteLine(patch);
        }
    }

    private static async Task HandleLastCommand()
    {
        var repository = await LoadRepository();
        Console.WriteLine(repository.GetLastExportedPatch());
    }

    private static async Task HandleImportCommand(string path)
    {
        var repository = await LoadRepository();
        var progress = new Progress<int>(x => Console.Write($"\rImporting...{x}%"));
        await repository.ImportPatch(path, progress);
        Console.WriteLine("\nDone!");
    }

    private static async Task HandleExportCommand(string patchVersion)
    {
        var repository = await LoadRepository();
        var progress = new Progress<int>(x => Console.Write($"\rExporting...{x}%"));
        await repository.ExportPatch(patchVersion, progress);
        Console.WriteLine("\nDone!");
    }

    private static async Task<LeagueVersionControl> LoadRepository()
    {
        var options = new LeagueVersionControlOptions()
        {
            RepositoryPath = @"D:\Temp\LVC",
            RegexFileFilters = new List<string>
            {
                @".*\.txt"
            }
        };

        return await LeagueVersionControl.Create(options);
    }
}