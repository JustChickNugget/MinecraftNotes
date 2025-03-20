using System.Text.Json.Serialization;

namespace MinecraftNotes.Structs.API;

public readonly struct GitHubApiResponse
{
    [JsonPropertyName("tag_name")]
    public required string TagName { get; init; }
}