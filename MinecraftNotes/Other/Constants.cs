using System;
using System.Diagnostics;
using System.IO;

namespace MinecraftNotes.Other;

/// <summary>
/// A static class that contains public constant variables for access throughout the application.
/// </summary>
public static class Constants
{
    /// <summary>
    /// JSON data file save location.
    /// </summary>
    public static string SavePath { get; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "JustChickNugget",
        "MinecraftNotes.json");

    /// <summary>
    /// Link to the developer's page.
    /// </summary>
    public static string DeveloperLink => "https://github.com/JustChickNugget";

    /// <summary>
    /// Link to the application's repository.
    /// </summary>
    public static string RepositoryLink { get; } = $"{DeveloperLink}/MinecraftNotes";

    /// <summary>
    /// API link used to retrieve latest release's information.
    /// </summary>
    public static string LatestReleaseApiLink =>
        "https://api.github.com/repos/JustChickNugget/MinecraftNotes/releases/latest";

    /// <summary>
    /// Process start info to open explorer with the location of the JSON data file.
    /// </summary>
    public static ProcessStartInfo SavePathProcessStartInfo { get; } = new()
    {
        FileName = Path.GetDirectoryName(SavePath),
        UseShellExecute = true
    };
}