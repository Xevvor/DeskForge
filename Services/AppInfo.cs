namespace DeskForge.Services;

/// <summary>
/// Single source of truth for the version shown in the sidebar footer.
/// Convention: bump the patch number by 0.0.1 for every future change made to this app.
/// </summary>
public static class AppInfo
{
    public const string Version = "v0.1.3";
}
