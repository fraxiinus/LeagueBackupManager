namespace Fraxiinus.LeagueBackupManager.LeagueVersionControl;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LeagueVersionControlOptions
{
    public string RepositoryIndexFile { get; set; } = "vcs.json";

    public string RepositoryPath { get; set; } = string.Empty;

    public List<string> RegexFileFilters { get; set; } = new List<string>();
}
