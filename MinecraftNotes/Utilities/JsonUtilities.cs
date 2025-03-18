using MinecraftNotes.Structs;
using MinecraftNotes.Other;
using System.IO;
using System.Text.Json;

namespace MinecraftNotes.Utilities;

public static class JsonUtilities
{
    private static JsonSerializerOptions SerializerOptions { get; } = new()
    {
        WriteIndented = true
    };

    public static void SaveWorldData(Dictionary<string, List<WorldPlace>> worlds)
    {
        // Save world data to the JSON file.
        
        string json = JsonSerializer.Serialize(worlds, SerializerOptions);
        File.WriteAllText(Variables.SavePath, json);
    }
    
    public static Dictionary<string, List<WorldPlace>> AppendWorldData(string worldName, WorldPlace worldPlace)
    {
        // Append world data to the JSON file.
        
        if (!Directory.Exists(Path.GetDirectoryName(Variables.SavePath)))
        {
            Directory.CreateDirectory
                (Path.GetDirectoryName(Variables.SavePath) ?? throw new InvalidOperationException("Directory path is null or empty."));
        }

        Dictionary<string, List<WorldPlace>> worlds;
        
        if (File.Exists(Variables.SavePath))
        {
            worlds = JsonSerializer.Deserialize<Dictionary<string, List<WorldPlace>>>
                         (File.ReadAllText(Variables.SavePath), SerializerOptions)
                     ?? new Dictionary<string, List<WorldPlace>>();

            if (worlds.TryGetValue(worldName, out List<WorldPlace>? worldPlaces) && !worldPlaces.Contains(worldPlace))
                worldPlaces.Add(worldPlace);
            else
                worlds[worldName] = [worldPlace];
        }
        else
        {
            worlds = new Dictionary<string, List<WorldPlace>>
            {
                { worldName, [worldPlace] }
            };
        }
        
        string json = JsonSerializer.Serialize(worlds, SerializerOptions);
        File.WriteAllText(Variables.SavePath, json);

        return worlds;
    }

    public static Dictionary<string, List<WorldPlace>> LoadWorldData()
    {
        // Load world data from the JSON file.
        
        if (!Directory.Exists(Path.GetDirectoryName(Variables.SavePath)))
        {
            Directory.CreateDirectory
                (Path.GetDirectoryName(Variables.SavePath) ?? throw new InvalidOperationException("Directory path is null or empty."));
        }

        if (!File.Exists(Variables.SavePath))
            return new Dictionary<string, List<WorldPlace>>();

        Dictionary<string, List<WorldPlace>> worlds = JsonSerializer.Deserialize<Dictionary<string, List<WorldPlace>>>
                                                          (File.ReadAllText(Variables.SavePath), SerializerOptions)
                                                      ?? throw new InvalidOperationException();

        return worlds;
    }
}