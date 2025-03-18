namespace MinecraftNotes.Structs;

public readonly record struct WorldPlace
{
    // World's place has a name and a location.
    
    public required string Name { get; init; }
    public required Location Location { get; init; }
}