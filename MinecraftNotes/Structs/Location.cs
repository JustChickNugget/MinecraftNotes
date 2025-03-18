namespace MinecraftNotes.Structs;

public readonly record struct Location
{
    /*
     * Location has Vector3D structure of X, Y, Z coordinates.
     * Use this structure to give a place in the world a location.
     */
    
    public required int X { get; init; }
    public required int Y { get; init; }
    public required int Z { get; init; }

    public override string ToString()
    {
        return $"X: {X}, Y: {Y}, Z: {Z}";
    }
}