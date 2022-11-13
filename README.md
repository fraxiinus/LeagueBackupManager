# League Backup Manager

Backup old versions of League of Legends to play old replays! Built for [ReplayBook](https://github.com/fraxiinus/ReplayBook).

## Using the CLI

Provides command line access to the backup repository.

### Import patches

Folder must contain `League of Legends.exe`

```powershell
.\LeagueBackupManager.exe import "C:\Path\To\Game"
```

### List available patches

```powershell
.\LeagueBackupManager.exe list
```

### Export patch to use

Version must match exactly

```powershell
.\LeagueBackupManager.exe export "version"
```

### Show last exported patch

```powershell
.\LeagueBackupManager.exe last
```

## Acknowledgements

[preyneyv](https://github.com/preyneyv/league-vcs) - Pioneered League-VCS technology

[JerryLiew](https://github.com/JerryLiew/LOL-VCS) - C# implementation
