namespace MinecraftNotes.Structs;

/// <summary>
/// Contains information about location with the X, Y, Z coordinates.
/// </summary>
public readonly record struct Location
{
    /// <summary>
    /// Location's X coordinate.
    /// </summary>
    public required int X { get; init; }

    /// <summary>
    /// Location's Y coordinate.
    /// </summary>
    public required int Y { get; init; }

    /// <summary>
    /// Location's Z coordinate.
    /// </summary>
    public required int Z { get; init; }
}