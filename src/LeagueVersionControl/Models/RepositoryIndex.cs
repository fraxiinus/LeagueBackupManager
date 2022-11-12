using System.Text.Json.Serialization;

namespace Fraxiinus.LeagueBackupManager.LeagueVersionControl.Models;

public class RepositoryIndex
{
    [JsonPropertyName("availablePatches")]
    public List<string> AvailablePatches { get; set; } = new List<string>();

    [JsonPropertyName("lastExportedPatch")]
    public string LastExportedPatch { get; set; } = string.Empty;

}
