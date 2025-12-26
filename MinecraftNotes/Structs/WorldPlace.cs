namespace MinecraftNotes.Structs;

/// <summary>
/// Contains information about the place in the world.
/// </summary>
public readonly record struct WorldPlace
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