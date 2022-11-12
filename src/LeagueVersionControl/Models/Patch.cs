namespace Fraxiinus.LeagueBackupManager.LeagueVersionControl;

using System.Collections.Generic;
using System.Text.Json.Serialization;

public class Patch
{
    public Patch()
    {
        FileHashes = new Dictionary<string, string>();
    }

    /// <summary>
    /// The version of the patch.
    /// </summary>
    [JsonPropertyName("version")]
    public string Version { get; set; } = string.Empty;

    /// <summary>
    /// Collection of all files needed in a patch, along with their MD5 hashes.
    /// </summary>
    [JsonPropertyName("fileHashes")]
    public Dictionary<string, string> FileHashes { get; set; }

    public override string ToString()
    {
        var hashes = string.Join('\n', FileHashes.Select(x => $"{x.Key} \t {x.Value}"));
        var returnString = $"Version: {Version}\n{hashes}";

        return returnString;
    }
}
