namespace Fraxiinus.LeagueBackupManager.LeagueVersionControl.Helpers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public static class RegexExtensions
{
    public static bool IsMatch(this List<Regex> regularExpressions, string input)
    {
        foreach (var regex in regularExpressions)
        {
            if (regex.IsMatch(input)) return true;
        }

        return false;
    }
}
