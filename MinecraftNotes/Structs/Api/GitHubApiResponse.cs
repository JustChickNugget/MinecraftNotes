using System.Text.Json.Serialization;

namespace MinecraftNotes.Structs.Api;

/// <summary>
/// Contains GitHub's API response attributes.
/// </summary>
public class GitHubApiResponse
{
    /// <summary>
    /// Tag name from the response.
    /// </summary>
    [JsonPropertyName("tag_name")]
    public required string TagName { get; init; }
}