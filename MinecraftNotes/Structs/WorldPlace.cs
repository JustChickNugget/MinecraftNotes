namespace MinecraftNotes.Structs;

/// <summary>
/// Contains information about the place in the world such as name and location.
/// </summary>
public readonly record struct WorldPlace
{
    public required string Name { get; init; }
    public required Location Location { get; init; }
}