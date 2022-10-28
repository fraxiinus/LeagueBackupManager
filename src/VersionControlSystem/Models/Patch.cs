namespace LeagueBackupManager.VersionControlSystem.Models;
using System.Collections.Generic;

public class Patch
{
    public Patch()
    {
        FileHashes = new Dictionary<string, string>();
    }

    public int Header { get; }

    public Dictionary<string, string> FileHashes { get; }
}
