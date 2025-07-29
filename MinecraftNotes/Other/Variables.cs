using System.Diagnostics;
using System.IO;

namespace MinecraftNotes.Other;

/// <summary>
/// A static class that contains public variables for access throughout the application.
/// </summary>
public static class Variables
{
    private static string DeveloperLink => "https://github.com/JustChickNugget";
    public static string RepositoryLink => $"{DeveloperLink}/MinecraftNotes";
    public static string SavePath => $@"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\JustChickNugget\MinecraftNotes.json";

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