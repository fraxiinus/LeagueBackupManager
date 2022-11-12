namespace Fraxiinus.LeagueBackupManager.LeagueVersionControl;

using System.Collections.Generic;
using System.Text.Json.Serialization;

public class Patch
{
    public Patch()
    {
        FileHashes = new Dictionary<string, string>();
        WadHeader = Array.Empty<byte>();
    }

    /// <summary>
    /// The version of the patch.
    /// </summary>
    [JsonPropertyName("version")]
    public string Version { get; set; } = string.Empty;

    /// <summary>
    /// All files that end with ".wad.client" have same four bytes header in a patch.
    /// Sometimes, the difference between two patches is just the header.
    /// </summary>
    [JsonPropertyName("wadHeader")]
    public byte[] WadHeader { get; set; }

    /// <summary>
    /// Collection of all files needed in a patch, along with their MD5 hashes.
    /// </summary>
    [JsonPropertyName("fileHashes")]
    public Dictionary<string, string> FileHashes { get; set; }

    public override string ToString()
    {
        var hashes = string.Join('\n', FileHashes.Select(x => $"{x.Key} \t {x.Value}"));
        var returnString = $"Version: {Version}\nWadHeader: {WadHeader}\n{hashes}";

        return returnString;
    }
}
