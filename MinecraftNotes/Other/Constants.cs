using System.Diagnostics;
using System.IO;

namespace MinecraftNotes.Other;

/// <summary>
/// A static class that contains public variables for access throughout the application.
/// </summary>
public static class Constants
{
    public static string SavePath { get; } = $@"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\JustChickNugget\MinecraftNotes.json";

    private static string DeveloperLink => "https://github.com/JustChickNugget";
    public static string RepositoryLink { get; } = $"{DeveloperLink}/MinecraftNotes";

    public static string ReleasesApiLink =>
        "https://api.github.com/repos/JustChickNugget/MinecraftNotes/releases/latest";

    public static ProcessStartInfo SavePathProcessStartInfo { get; } = new()
    {
        FileName = Path.GetDirectoryName(SavePath),
        UseShellExecute = true
    };
    
    public static ProcessStartInfo DeveloperGitHubProcessStartInfo { get; } = new()
    {
        FileName = DeveloperLink,
        UseShellExecute = true
    };
}