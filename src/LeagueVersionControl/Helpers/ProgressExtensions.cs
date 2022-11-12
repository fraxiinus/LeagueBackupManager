namespace Fraxiinus.LeagueBackupManager.LeagueVersionControl.Helpers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ProgressExtensions
{
    public static void NullSafeReport(this IProgress<int>? progress, int value)
    {
        if (progress != null)
        {
            progress.Report(value);
        }
    }
}
