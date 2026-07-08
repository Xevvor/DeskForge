using DeskForge.Models;

namespace DeskForge.Services;

/// <summary>
/// A large pre-listed catalog of popular PC games shown in "Find Game". Each entry carries
/// the real-world install locations DeskForge checks during detection: Steam library paths
/// are resolved dynamically (including custom library drives), while launcher-specific games
/// (Epic, Riot, Battle.net, standalone) use their well-known default install folders. Known
/// Steam titles also carry a SteamAppId, used by GameArtworkService to pull real cover art
/// straight from Steam's CDN.
/// </summary>
public static class GameCatalogService
{
    public static List<GameCatalogEntry> All { get; } = new()
    {
        new GameCatalogEntry
        {
            Name = "Grand Theft Auto V", Icon = "🚗", AccentColor = "#5C9DFF", SteamAppId = 271590,
            SteamRelativePath = @"Grand Theft Auto V\GTA5.exe",
            AbsoluteCandidates = { @"%ProgramFiles%\Epic Games\GTAV\GTA5.exe", @"%ProgramFiles%\Rockstar Games\Grand Theft Auto V\GTA5.exe" }
        },
        new GameCatalogEntry
        {
            Name = "FiveM", Icon = "🏙️", AccentColor = "#FFA65C",
            AbsoluteCandidates = { @"%LocalAppData%\FiveM\FiveM Application Data\FiveM.exe" }
        },
        new GameCatalogEntry
        {
            Name = "Minecraft", Icon = "⛏️", AccentColor = "#8CE99A",
            AbsoluteCandidates = { @"%ProgramFiles(x86)%\Minecraft Launcher\MinecraftLauncher.exe", @"%ProgramFiles%\Minecraft Launcher\MinecraftLauncher.exe" }
        },
        new GameCatalogEntry
        {
            Name = "Fortnite", Icon = "🏆", AccentColor = "#D65CFF",
            AbsoluteCandidates = { @"%ProgramFiles%\Epic Games\Fortnite\FortniteGame\Binaries\Win64\FortniteClient-Win64-Shipping.exe" }
        },
        new GameCatalogEntry
        {
            Name = "VALORANT", Icon = "🛡️", AccentColor = "#FF5C5C",
            AbsoluteCandidates = { @"C:\Riot Games\VALORANT\live\VALORANT.exe" }
        },
        new GameCatalogEntry
        {
            Name = "League of Legends", Icon = "⚔️", AccentColor = "#5CE1E6",
            AbsoluteCandidates = { @"C:\Riot Games\League of Legends\LeagueClient.exe" }
        },
        new GameCatalogEntry
        {
            Name = "Counter-Strike 2", Icon = "💣", AccentColor = "#FFD65C", SteamAppId = 730,
            SteamRelativePath = @"Counter-Strike Global Offensive\game\bin\win64\cs2.exe"
        },
        new GameCatalogEntry
        {
            Name = "Apex Legends", Icon = "🔺", AccentColor = "#FF5CA8", SteamAppId = 1172470,
            SteamRelativePath = @"Apex Legends\r5apex.exe",
            AbsoluteCandidates = { @"%ProgramFiles%\EA Games\Apex Legends\r5apex.exe" }
        },
        new GameCatalogEntry
        {
            Name = "Call of Duty", Icon = "🎯", AccentColor = "#8C5CFF",
            AbsoluteCandidates = { @"%ProgramFiles(x86)%\Call of Duty\cod.exe" }
        },
        new GameCatalogEntry
        {
            Name = "Roblox", Icon = "🧱", AccentColor = "#5CFFB8",
            AbsoluteCandidates = { @"%LocalAppData%\Roblox\Versions\RobloxPlayerBeta.exe" }
        },
        new GameCatalogEntry
        {
            Name = "Rocket League", Icon = "🚀", AccentColor = "#5C9DFF", SteamAppId = 252950,
            SteamRelativePath = @"rocketleague\Binaries\Win64\RocketLeague.exe",
            AbsoluteCandidates = { @"%ProgramFiles%\Epic Games\rocketleague\Binaries\Win64\RocketLeague.exe" }
        },
        new GameCatalogEntry
        {
            Name = "Among Us", Icon = "🛸", AccentColor = "#FF5C5C", SteamAppId = 945360,
            SteamRelativePath = @"Among Us\Among Us.exe"
        },
        new GameCatalogEntry
        {
            Name = "Genshin Impact", Icon = "⚔️", AccentColor = "#FFD65C",
            AbsoluteCandidates = { @"C:\Genshin Impact game\Genshin Impact Game\GenshinImpact.exe" }
        },
        new GameCatalogEntry
        {
            Name = "Overwatch 2", Icon = "🦾", AccentColor = "#FFA65C",
            AbsoluteCandidates = { @"%ProgramFiles(x86)%\Overwatch\_retail_\Overwatch.exe" }
        },
        new GameCatalogEntry
        {
            Name = "Rainbow Six Siege", Icon = "🐻", AccentColor = "#8CE99A", SteamAppId = 359550,
            SteamRelativePath = @"Tom Clancy's Rainbow Six Siege\RainbowSix.exe",
            AbsoluteCandidates = { @"%ProgramFiles(x86)%\Ubisoft\Ubisoft Game Launcher\games\Tom Clancy's Rainbow Six Siege\RainbowSix.exe" }
        },
        new GameCatalogEntry
        {
            Name = "PUBG: Battlegrounds", Icon = "🏝️", AccentColor = "#5CE1E6", SteamAppId = 578080,
            SteamRelativePath = @"PUBG\TslGame\Binaries\Win64\TslGame.exe"
        },
        new GameCatalogEntry
        {
            Name = "Warframe", Icon = "🌌", AccentColor = "#D65CFF", SteamAppId = 230410,
            SteamRelativePath = @"Warframe\Downloaded\Public\Tools\Launcher.exe",
            AbsoluteCandidates = { @"%ProgramFiles(x86)%\Warframe\Downloaded\Public\Tools\Launcher.exe" }
        },
        new GameCatalogEntry
        {
            Name = "Terraria", Icon = "🌳", AccentColor = "#8CE99A", SteamAppId = 105600,
            SteamRelativePath = @"Terraria\Terraria.exe"
        },
        new GameCatalogEntry
        {
            Name = "Stardew Valley", Icon = "🚜", AccentColor = "#FFD65C", SteamAppId = 413150,
            SteamRelativePath = @"Stardew Valley\Stardew Valley.exe"
        },
        new GameCatalogEntry
        {
            Name = "ARK: Survival Evolved", Icon = "🦖", AccentColor = "#FFA65C", SteamAppId = 346110,
            SteamRelativePath = @"ARK\ShooterGame\Binaries\Win64\ShooterGame.exe"
        },
        new GameCatalogEntry
        {
            Name = "Rust", Icon = "🔧", AccentColor = "#FF5C5C", SteamAppId = 252490,
            SteamRelativePath = @"Rust\RustClient.exe"
        },
        new GameCatalogEntry
        {
            Name = "The Sims 4", Icon = "🏡", AccentColor = "#5CFFB8",
            AbsoluteCandidates = { @"%ProgramFiles(x86)%\Origin Games\The Sims 4\Game\Bin\TS4_x64.exe", @"%ProgramFiles%\EA Games\The Sims 4\Game\Bin\TS4_x64.exe" }
        },
        new GameCatalogEntry
        {
            Name = "World of Warcraft", Icon = "🐉", AccentColor = "#5C9DFF",
            AbsoluteCandidates = { @"C:\Program Files (x86)\World of Warcraft\_retail_\Wow.exe" }
        },
        new GameCatalogEntry
        {
            Name = "Destiny 2", Icon = "🌠", AccentColor = "#8C5CFF", SteamAppId = 1085660,
            SteamRelativePath = @"Destiny 2\destiny2.exe"
        },
        new GameCatalogEntry
        {
            Name = "Escape from Tarkov", Icon = "🎒", AccentColor = "#FFA65C",
            AbsoluteCandidates = { @"C:\Battlestate Games\EFT\EscapeFromTarkov.exe" }
        },
        new GameCatalogEntry
        {
            Name = "Dota 2", Icon = "🪓", AccentColor = "#FF5C5C", SteamAppId = 570,
            SteamRelativePath = @"dota 2 beta\game\bin\win64\dota2.exe"
        },
        new GameCatalogEntry
        {
            Name = "Team Fortress 2", Icon = "🧢", AccentColor = "#FFD65C", SteamAppId = 440,
            SteamRelativePath = @"Team Fortress 2\hl2.exe"
        },
        new GameCatalogEntry
        {
            Name = "Forza Horizon 5", Icon = "🏎️", AccentColor = "#5CE1E6", SteamAppId = 1551360,
            SteamRelativePath = @"Forza Horizon 5\ForzaHorizon5.exe"
        },
        new GameCatalogEntry
        {
            Name = "Cyberpunk 2077", Icon = "🤖", AccentColor = "#FFD65C", SteamAppId = 1091500,
            SteamRelativePath = @"Cyberpunk 2077\bin\x64\Cyberpunk2077.exe"
        },
        new GameCatalogEntry
        {
            Name = "Elden Ring", Icon = "🗡️", AccentColor = "#D65CFF", SteamAppId = 1245620,
            SteamRelativePath = @"ELDEN RING\Game\eldenring.exe"
        },
        new GameCatalogEntry
        {
            Name = "Red Dead Redemption 2", Icon = "🤠", AccentColor = "#FFA65C", SteamAppId = 1174180,
            SteamRelativePath = @"Red Dead Redemption 2\RDR2.exe"
        },
    };
}
