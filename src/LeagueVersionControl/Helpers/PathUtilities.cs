namespace Fraxiinus.LeagueBackupManager.LeagueVersionControl.Helpers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class PathUtilities
{
    public static string GetRelativePath(string relativeTo, string fullPath)
    {
        int relativePathLength = relativeTo.Length;
        int fullPathLength = fullPath.Length;
        string substring = fullPath.Substring(relativePathLength + 1, fullPathLength - 1 - relativePathLength);
        return substring;
    }
}
