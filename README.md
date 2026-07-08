# DeskForge

A modern, dark-themed Windows desktop toolbox built with **C# / WPF / .NET 9**. DeskForge bundles a dashboard, a game launcher, a file cleaner, a file organizer, and a handful of quick utilities into one compact app.

> **Status:** prototype (v0.1.3). Core flows are real and functional — see [What's real vs. simulated](#whats-real-vs-simulated) below.

## Features

- **Dashboard** — live stats (files scanned, games added, storage "saved") and one-click shortcuts into the other tools.
- **Launcher** — a game-focused launcher. Browse a 30-title catalog (GTA V, Fortnite, Minecraft, Valorant, Elden Ring, and more), and DeskForge scans your PC for a real install — including every Steam library folder, not just the default one. If it can't find a game automatically, it asks whether to add it manually via a file picker. Added games show real cover art (Steam CDN, or RAWG.io if you supply a key) and persist across restarts.
- **File Cleaner** — scans a real folder on disk and flags large files, old files, duplicates, and temp files. Read-only: nothing is deleted without an explicit confirmation, and deletion is currently simulated (see below).
- **File Organizer** — previews how a folder would be sorted by file type (Images → Pictures, Videos → Videos, etc.) without moving anything.
- **Quick Tools** — a scratchpad note (saved to disk), quick links to common folders/sites, and one-click system shortcuts (Task Manager, Control Panel, Settings, Recycle Bin).

The whole app uses a custom-drawn dark title bar, animated page transitions, and animated loading indicators instead of native/instant UI.

## Running it

**Option A — run the published build (no .NET SDK required):**

1. Install the [.NET 9 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/9.0) if you don't already have it (Windows only).
2. Run [`publish/DeskForge.exe`](publish/DeskForge.exe).

**Option B — build from source:**

```
dotnet run
```

Requires the [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) (or newer) on Windows.

### Optional: real artwork for non-Steam games

Steam titles get real cover art automatically (no setup needed — DeskForge builds the URL from Steam's public CDN). Everything else (Fortnite, Minecraft, Valorant, etc.) can pull real art from [RAWG.io](https://rawg.io/apidocs) if you provide a free API key, via **either**:

- an environment variable: `DESKFORGE_RAWG_API_KEY=your-key-here`, or
- `%AppData%\DeskForge\settings.json`:
  ```json
  { "RawgApiKey": "your-key-here" }
  ```

Without a key, those tiles just show a generated color+icon placeholder instead — the app works fine either way.

## Project structure

```
DeskForge/
├── App.xaml(.cs)          — app entry point, resource dictionaries, ViewModel→View mapping
├── MainWindow.xaml(.cs)    — app shell: custom title bar, sidebar, page host
├── Controls/               — AnimatedContentControl (page-transition behavior)
├── Converters/              — WPF value converters (visibility/enum helpers)
├── Models/                 — plain data types (AppShortcut, GameCatalogEntry, ScannedFile, ...)
├── ViewModels/              — one view model per page + MVVM base classes (MVVM, no framework dependency)
├── Views/                  — one XAML view per page
├── Services/                — file scanning, game detection, artwork lookup, persistence, etc.
├── Themes/DarkTheme.xaml    — the whole visual language: palette, styles, animations
└── publish/                 — a ready-to-run build (see "Running it" above)
```

## What's real vs. simulated

| Feature | Status |
|---|---|
| File Cleaner scan | **Real** — walks the folder you pick, flags files by real size/age/duplicate rules |
| Game detection | **Real** — checks actual Steam library folders (parsed from `libraryfolders.vdf`) plus known Epic/Riot/Battle.net/standalone paths |
| Game artwork | **Real** — Steam CDN always; RAWG.io if you configure a key |
| Launching apps/games | **Real** — actually starts the process |
| Your Games list | **Real** — persisted to `%AppData%\DeskForge\my_games.json` |
| Clipboard notes | **Real** — persisted to `%AppData%\DeskForge\notes.txt` |
| File Cleaner "Clean Selected" | **Simulated** — requires confirmation, never deletes real files (by design, for safety) |
| File Organizer | **Preview-only** — computes what *would* move, never calls `File.Move` |

## Tech notes

- MVVM with hand-rolled `RelayCommand` / `ViewModelBase` — no external MVVM framework dependency.
- No third-party NuGet packages. Networking (RAWG) and JSON (settings/cache/library files) use only the .NET BCL (`HttpClient`, `System.Text.Json`).
- Custom window chrome via `System.Windows.Shell.WindowChrome` (native minimize/maximize/resize/snap behavior, fully custom-drawn buttons).

## License

[MIT](LICENSE).
