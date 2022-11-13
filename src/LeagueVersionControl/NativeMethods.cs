namespace Fraxiinus.LeagueBackupManager.LeagueVersionControl;

using System.Runtime.InteropServices;

public static class NativeMethods
{
    [DllImport("Kernel32", CharSet = CharSet.Unicode)]
    internal static extern bool CreateHardLink(string linkName, string sourceName, IntPtr attribute);
}
