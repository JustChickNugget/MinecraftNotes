using System;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using MinecraftNotes.Models.Api;
using MinecraftNotes.Other;
using MinecraftNotes.Windows;

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
    /// <param name="cancellationToken">Cancellation token object</param>
    public static async Task<bool> CheckForUpdatesAsync(CancellationToken cancellationToken = default)
    {
        using HttpClient client = new();
        client.DefaultRequestHeaders.Add("User-Agent", "NuggetLib");

        string response = await client.GetStringAsync(Constants.LatestReleaseApiLink, cancellationToken);
        GitHubApiResponse? gitHubApiResponse = JsonSerializer.Deserialize<GitHubApiResponse>(response);

        if (gitHubApiResponse == null)
        {
            throw new ArgumentNullException(null, "GitHub API response is null.");
        }

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
    /// <param name="ownerWindow">Window in which the exception occurred</param>
    /// <param name="exception">Exception object</param>
    /// <param name="className">Name of the class where the exception occurred</param>
    /// <param name="functionName">Name of the function where the exception occurred</param>
    /// <param name="onlyDebugPrintException">Only debug print the exception without creating an exception window</param>
    public static async Task PrintException(
        Window ownerWindow,
        Exception exception,
        string className,
        string functionName,
        bool onlyDebugPrintException = false)
    {
#if DEBUG
        Debug.WriteLine($"[{className}] -> ({functionName}): \"{exception.Message}\"");
#endif

        if (onlyDebugPrintException)
        {
            return;
        }

        ExceptionWindow exceptionWindow = new(exception, className, functionName);
        await exceptionWindow.ShowDialog(ownerWindow);
    }
}