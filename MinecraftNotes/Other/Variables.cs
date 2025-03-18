using System.Diagnostics;
using System.IO;

namespace MinecraftNotes.Other;

internal static class Variables
{
    internal static string SavePath { get; } = $@"C:\Users\{Environment.UserName}\AppData\Local\JustChickNugget\MinecraftNotes.json";

    internal static ProcessStartInfo SavePathProcessStartInfo { get; } = new()
    {
        FileName = Path.GetDirectoryName(SavePath),
        UseShellExecute = true
    };
    
    internal static ProcessStartInfo GitHubProcessStartInfo { get; } = new()
    {
        FileName = "https://github.com/JustChickNugget",
        UseShellExecute = true
    };
}