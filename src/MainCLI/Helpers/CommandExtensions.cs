namespace Fraxiinus.LeagueBackupManager.MainCLI.Helpers;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.Reflection;

public static class CommandExtensions
{
    public static Command WithHandler(this Command command, string methodName)
    {
        var method = typeof(Program).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);
        var handler = CommandHandler.Create(method!);
        command.Handler = handler;
        return command;
    }
}
