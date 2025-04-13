using System.Diagnostics;
using System.IO;

namespace MinecraftNotes.Other;

internal static class Variables
{
    private static string DeveloperLink => "https://github.com/JustChickNugget";
    internal static string RepositoryLink => $"{DeveloperLink}/MinecraftNotes";
    internal static string SavePath => $@"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\JustChickNugget\MinecraftNotes.json";

    internal static ProcessStartInfo SavePathProcessStartInfo { get; } = new()
    {
        FileName = Path.GetDirectoryName(SavePath),
        UseShellExecute = true
    };
    
    internal static ProcessStartInfo DeveloperGitHubProcessStartInfo { get; } = new()
    {
        FileName = DeveloperLink,
        UseShellExecute = true
    };
}