using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using MinecraftNotes.Other;
using MinecraftNotes.Structs.API;

#if DEBUG
using System.Diagnostics;
#endif

namespace MinecraftNotes.Utilities;

/// <summary>
/// Used for common functions that are repeated and used many times throughout the application.
/// </summary>
public static class ToolBox
{
    /// <summary>
    /// Check for updates using GitHub's API.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel operations.</param>
    public static async Task<bool> CheckForUpdates(CancellationToken cancellationToken = default)
    {
        using HttpClient client = new();
        client.DefaultRequestHeaders.Add("User-Agent", "NuggetLib");

        string response = await client.GetStringAsync(Constants.ReleasesApiLink, cancellationToken);
        GitHubApiResponse gitHubApiResponse = JsonSerializer.Deserialize<GitHubApiResponse>(response);

        if (gitHubApiResponse.TagName == null)
        {
            throw new ArgumentNullException(null, "Couldn't check version: tag name is null.");
        }

        Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version ?? new Version(1, 0, 0, 0);

        string latestReleaseVersionString = gitHubApiResponse.TagName.StartsWith('v')
            ? gitHubApiResponse.TagName[1..]
            : gitHubApiResponse.TagName;

        latestReleaseVersionString = latestReleaseVersionString.Split('.').Length == 3
            ? latestReleaseVersionString + ".0"
            : latestReleaseVersionString;

        if (!Version.TryParse(latestReleaseVersionString, out Version? latestReleaseVersion))
        {
            throw new FormatException($"Invalid version format: {gitHubApiResponse.TagName}");
        }

        return latestReleaseVersion > currentVersion;
    }

    /// <summary>
    /// Print exception if it has occurred.
    /// </summary>
    /// <param name="ex">An object containing the exception stack trace.</param>
    public static void PrintException(Exception ex)
    {
        string title = $"An exception has occurred ({ex.GetType().FullName})";

#if DEBUG
        string exceptionString = ex.ToString();

        MessageBox.Show(exceptionString, title, MessageBoxButton.OK, MessageBoxImage.Error);
        Debug.WriteLine(exceptionString);
#else
        MessageBox.Show(ex.Message, title, MessageBoxButton.OK, MessageBoxImage.Error);
#endif
    }
}