namespace Fraxiinus.LeagueBackupManager.LeagueVersionControl;

using Fraxiinus.LeagueBackupManager.LeagueVersionControl.Helpers;
using Fraxiinus.LeagueBackupManager.LeagueVersionControl.Models;
using System.Diagnostics;
using System.Text.Json;
using System.Text.RegularExpressions;

public class LeagueVersionControl
{
    private readonly LeagueVersionControlOptions _options;

    private RepositoryIndex _repositoryIndex;

    #region Public Methods

    private LeagueVersionControl(LeagueVersionControlOptions options)
    {
        _options = options;
        _repositoryIndex = new RepositoryIndex();
    }

    public static async Task<LeagueVersionControl> Create(LeagueVersionControlOptions options)
    {
        var leagueVersionControl = new LeagueVersionControl(options);
        await leagueVersionControl.CheckOrInitializeRepository();
        return leagueVersionControl;
    }

    /// <summary>
    /// Get all patches available in repository, null if not loaded
    /// </summary>
    /// <returns></returns>
    public string[] GetAvailablePatches() => _repositoryIndex?.AvailablePatches?.ToArray() ?? Array.Empty<string>();

    public string GetLastExportedPatch() => _repositoryIndex?.LastExportedPatch ?? string.Empty;

    /// <summary>
    /// Loads the provided league of legends copy into repository
    /// </summary>
    /// <param name="inputLolPath"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task ImportPatch(string inputLolPath,
        IProgress<int>? progress = default,
        CancellationToken cancellationToken = default)
    {
        // 1. Check if trying to import a version that already exists
        var (patchExists, patchVersion) = CheckLeagueVersionExists(inputLolPath);
        if (patchExists)
        {
            throw new Exception($"Patch {patchVersion} already exists");
        }
        if (patchVersion == null)
        {
            throw new Exception("Patch version is null (???)");
        }

        // 2. Initialize regex file filters
        List<Regex> regexFileFilters = _options.RegexFileFilters.Select(x => new Regex(x)).ToList();

        // 3. Add league files to repository
        var patch = await CreatePatch(patchVersion!, inputLolPath, regexFileFilters, progress, cancellationToken);

        // 4. Update collections
        _repositoryIndex!.AvailablePatches.Add(patchVersion!);
        await WriteRepositoryIndex();
    }

    public async Task<string> ExportPatch(string patchVersion,
        IProgress<int>? progress = default,
        CancellationToken cancellationToken = default)
    {
        // 1. Check if trying to export a version that does not exist
        if (!_repositoryIndex.AvailablePatches.Contains(patchVersion))
        {
            throw new Exception($"Patch {patchVersion} does not exist");
        }

        // 2. Load patch file
        var patch = await ReadPatchFile(patchVersion);

        // 3. Copy files / create links to export directory
        var lolExePath = string.Empty;
        // setup progress
        var progressCount = 0;
        var progressLimit = patch.FileHashes.Count;
        progress.NullSafeReport(progressCount);

        foreach (var (destinationRelativePath, hash) in patch.FileHashes)
        {
            var destinationFilePath = Path.Combine(_options.RepositoryPath, "export", destinationRelativePath);
            // Ensure if destination folder exists
            var destinationDirectory = Path.GetDirectoryName(destinationFilePath);
            if (!Directory.Exists(destinationDirectory) && !string.IsNullOrEmpty(destinationDirectory))
            {
                Directory.CreateDirectory(destinationDirectory);
            }

            // Create/overwrite destination file
            using FileStream destinationStream = new(destinationFilePath, FileMode.Create);

            // Copy source file from repository
            using FileStream sourceStream = new(GetRepositoryFilePath(hash), FileMode.Open);
            await sourceStream.CopyToAsync(destinationStream, cancellationToken);

            if (string.IsNullOrEmpty(lolExePath) && destinationFilePath.EndsWith("League of Legends.exe"))
            {
                lolExePath = destinationFilePath;
            }

            // Report progress
            progress.NullSafeReport(progressCount * 100 / progressLimit);
        }

        // remember what patch is currently exported
        _repositoryIndex.LastExportedPatch = patchVersion;
        await WriteRepositoryIndex();
        // 4. Return executable path
        return lolExePath;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Contains patch index files
    /// </summary>
    /// <returns></returns>
    private string GetPatchFolder() => Path.Combine(_options.RepositoryPath, "patch");

    /// <summary>
    /// Get patch index file for given patch version
    /// </summary>
    /// <param name="patchVersion"></param>
    /// <returns></returns>
    private string GetPatchFilePath(string patchVersion) => Path.Combine(GetPatchFolder(), patchVersion + ".json");

    private string GetRepositoryIndex() => Path.Combine(_options.RepositoryPath, _options.RepositoryIndexFile);

    /// <summary>
    /// Get file from repository from md5
    /// </summary>
    /// <param name="md5Str"></param>
    /// <returns></returns>
    private string GetRepositoryFilePath(string md5Str) => Path.Combine(_options.RepositoryPath, "files", md5Str);

    /// <summary>
    /// Ensure repository is set up
    /// </summary>
    private async Task CheckOrInitializeRepository()
    {
        var repositoryIndexFilePath = GetRepositoryIndex();

        // database file exists
        if (File.Exists(repositoryIndexFilePath))
        {
            await ReadRepositoryIndex(repositoryIndexFilePath);
            return;
        }

        // does not exist, create blank file and folders
        File.WriteAllText(repositoryIndexFilePath, "{}");
        _repositoryIndex = new RepositoryIndex();
        Directory.CreateDirectory(Path.Combine(_options.RepositoryPath, "files"));
        Directory.CreateDirectory(GetPatchFolder());
    }

    /// <summary>
    /// Given the path to the GAME folder, finds version and returns it.
    /// </summary>
    /// <param name="inputLolPath"></param>
    /// <returns></returns>
    private (bool exists, string? version) CheckLeagueVersionExists(string inputLolPath)
    {
        var targetFile = Path.Combine(inputLolPath, "League of Legends.exe");
        if (!File.Exists(targetFile))
        {
            throw new FileNotFoundException(null, targetFile);
        }

        var patchVersion = FileVersionInfo.GetVersionInfo(targetFile).ProductVersion
            ?? throw new Exception("Version information was not found");

        return (_repositoryIndex.AvailablePatches.Contains(patchVersion), patchVersion);
    }

    private async Task<Patch> CreatePatch(
        string patchVersion,
        string inputLolPath,
        List<Regex> regexFileFilters,
        IProgress<int>? progress = default,
        CancellationToken cancellationToken = default)
    {
        var hashes = await AddToRepository(inputLolPath, regexFileFilters, progress, cancellationToken);

        var patch = new Patch()
        {
            Version = patchVersion,
            FileHashes = hashes
        };

        var patchFile = GetPatchFilePath(patchVersion);
        using FileStream fileStream = new(patchFile, FileMode.Create);
        await JsonSerializer.SerializeAsync<Patch>(fileStream, patch, options: null, cancellationToken);

        return patch;
    }

    private async Task<Dictionary<string, string>> AddToRepository(
        string inputLolPath,
        List<Regex> regexFileFilters,
        IProgress<int>? progress = default,
        CancellationToken cancellationToken = default)
    {
        var rootDirectory = new DirectoryInfo(inputLolPath);
        var hashes = new Dictionary<string, string>();
        // enumerate all files, including subdirectories
        var allFiles = rootDirectory.EnumerateFiles("*", SearchOption.AllDirectories);
        // setup progress reporting
        int currentProgress = 0;
        int totalFiles = allFiles.Count();
        progress.NullSafeReport(currentProgress);
        foreach (var file in allFiles)
        {
            var relativePath = PathUtilities.GetRelativePath(inputLolPath, file.FullName);
            // Skip file if relative path matches regex filter
            if (regexFileFilters.IsMatch(relativePath)) { continue; }

            using FileStream fileStream = new(file.FullName, FileMode.Open);

            // Find potential destination file path, wad files skip first 4 bytes
            var md5Hash = await fileStream.ComputeMD5Hash();
            hashes.Add(relativePath, md5Hash);

            var destinationFile = GetRepositoryFilePath(md5Hash);

            // copy to repository if not exists
            if (!File.Exists(destinationFile))
            {
                // reset filestream
                fileStream.Seek(0, SeekOrigin.Begin);

                if (file.Name.EndsWith(".wad.client"))
                {
                    // skip 4 bytes when writing wad files, to be filled by wadHeader
                    fileStream.Seek(4, SeekOrigin.Begin);
                }

                // create destination stream and copy async
                using FileStream destinationStream = new(destinationFile, FileMode.Create);
                await fileStream.CopyToAsync(destinationStream, cancellationToken);
            }

            // report on current progress
            currentProgress++;
            progress.NullSafeReport(currentProgress * 100 / totalFiles);
        }

        return hashes;
    }

    private async Task ReadRepositoryIndex(string repositoryIndexFilePath)
    {
        using FileStream index = new(repositoryIndexFilePath, FileMode.Open);
        _repositoryIndex = await JsonSerializer.DeserializeAsync<RepositoryIndex>(index)
            ?? throw new Exception("Failed to deserialize repository index");
    }

    private async Task WriteRepositoryIndex()
    {
        var repositoryIndexFilePath = GetRepositoryIndex();
        if (_repositoryIndex == null)
        {
            throw new Exception("Repository index is null, cannot be written");
        }

        using FileStream index = new(repositoryIndexFilePath, FileMode.Create);
        await JsonSerializer.SerializeAsync<RepositoryIndex>(index, _repositoryIndex);
    }

    private async Task<Patch> ReadPatchFile(string patchVersion)
    {
        var patchFilePath = GetPatchFilePath(patchVersion);
        using FileStream fileStream = new(patchFilePath, FileMode.Open);
        return await JsonSerializer.DeserializeAsync<Patch>(fileStream)
            ?? throw new Exception("Failed to deserialize patch file");
    }

    #endregion
}