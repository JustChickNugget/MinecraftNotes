namespace MinecraftNotes.Structs;

/// <summary>
/// Contains information about location with the X, Y, Z coordinates.
/// </summary>
public readonly record struct Location
{
    public required int X { get; init; }
    public required int Y { get; init; }
    public required int Z { get; init; }

    /// <summary>
    /// Converts a structure to a string.
    /// </summary>
    /// <returns>A string of location coordinates in the format: "X: ..., Y: ..., Z: ..."</returns>
    public override string ToString()
    {
        return $"X: {X}, Y: {Y}, Z: {Z}";
    }
}