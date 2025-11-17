using System.Text.Json.Serialization;

namespace MinecraftNotes.Structs.API;

/// <summary>
/// Contains latest tag name which is marked as version.
/// </summary>
public readonly struct GitHubApiResponse
{
    [JsonPropertyName("tag_name")] public required string TagName { get; init; }
}