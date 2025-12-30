namespace MinecraftNotes.Models.Minecraft;

/// <summary>
/// Contains information about the place in the world.
/// </summary>
public sealed record WorldPlace
{
    /// <summary>
    /// Name of the place.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Location of the place in the world.
    /// </summary>
    public required Location Location { get; init; }
}