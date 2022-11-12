﻿using System.Text.Json.Serialization;

namespace Fraxiinus.LeagueBackupManager.LeagueVersionControl.Models;

public class RepositoryIndex
{
    [JsonPropertyName("availablePatches")]
    public List<string> AvailablePatches { get; set; } = new List<string>();
}