namespace LeagueBackupManager.VersionControlSystem;
using System.Text.RegularExpressions;

public class LeagueVCS
{
    private const string DB_NAME = "vcs.json";
    private readonly string _repositoryPath;
    private List<Regex> _filters = new();

    public LeagueVCS(string repoPath)
    {
        _repositoryPath = repoPath;
    }
}