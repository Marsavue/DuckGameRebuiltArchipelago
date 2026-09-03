# Duck Game Arcade Archipelago
This project is based off of Duck Game Rebuilt.

### AI Disclosure
Nothing I make is ever developed using AI, that does not necessarily include Duck Game Rebuilt

## Setup Guide
You MUST own the game on steam and steam MUST be open
1. Download latest version from [Releases](https://github.com/Marsavue/DuckGameRebuiltArchipelago/releases/latest) & unzip
2. Windows run DuckGame.exe - Linux run DuckGame.sh (need mono installed)
3. Once game is open, configure slot settings in Pause>Options>Archipelago
4. Walk into Arcade to connect

## As of v0.0.4
- Levels and Weapons are items you can receive
- Locations are medals of your choice
- Several simple yaml options

APWorld - https://github.com/Marsavue/Archipelago-DuckGame/releases/latest

[Check pins in the Archipelago discord for more info](https://discord.com/channels/731205301247803413/1526871890226974750)

# <img src="icon.png" height="32"> Duck Game Rebuilt

Duck Game Rebuilt is a decompilation of Duck Game with massive improvements to performance, compatibility, and quality of life features.

## For Developers 🚧

Welcome to the repo, enjoy your stay, please unfuck the code. thanks

> [!NOTE]
> Your IDE will scream at you with 200+ warnings when building. That's normal.

### Prerequisites

| Platform | Required |
|---|---|
| Windows | [Visual Studio 2022](https://visualstudio.microsoft.com/vs/community/) with the **".NET desktop development"** workload (includes MSBuild, NuGet, and the [.NET Framework 4.8 targeting pack](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net48)) |
| Linux | `mono-complete` from the [official mono repos](https://www.mono-project.com/download/stable/), plus the `nuget` CLI |
| All | [Steam](https://store.steampowered.com/) running at launch time; clone with `--recursive` for submodules |

### Cloning

This repository uses git submodules. Either clone with the `--recursive` flag, or run this command after a normal clone:

```bash
git submodule update --init --recursive
```

### Building on Windows

1. Open `DuckGame.sln` in Visual Studio 2022.
2. Restore NuGet packages (most IDEs do this automatically).
3. Set **DuckGame** as the startup project — not CrashWindow, FNA, Rebuilder, or anything else.
4. Build the solution with `Ctrl+Shift+B`.

### Building on GNU/Linux

1. Add the [official monoproject repos](https://www.mono-project.com/download/stable/) (unless you're firebreak, apparently).
2. Install the `mono-complete` package.
3. `cd` to the solution's directory.
4. Restore NuGet packages if your IDE hasn't:
   ```bash
   nuget restore
   ```
5. Copy DLL dependencies from `./DuckGame/lib/` into `./bin/`:
   ```bash
   mkdir ./bin/
   cp ./DuckGame/lib/* ./bin/
   ```
6. Build the solution:
   ```bash
   msbuild -m -p:Configuration=Debug
   ```

### Running

> [!IMPORTANT]
> Steam must be running before launching the game, or it will crash on startup.

- **Windows** — press `F5` in Visual Studio to launch in Debug mode.
- **Linux** — run the built executable under mono:
  ```bash
  mono ./bin/DuckGame.exe
  ```
