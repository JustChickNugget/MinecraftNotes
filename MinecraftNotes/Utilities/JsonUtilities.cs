using MinecraftNotes.Structs;
using MinecraftNotes.Other;
using System.IO;
using System.Text.Json;

namespace MinecraftNotes.Utilities;

/// <summary>
/// Static class containing functions to easily work with the JSON and worlds.
/// </summary>
public static class JsonUtilities
{
    private static JsonSerializerOptions SerializerOptions { get; } = new()
    {
        WriteIndented = true
    };

    /// <summary>
    /// Save world data to the JSON file.
    /// </summary>
    /// <param name="worlds">Dictionary, that contains information about worlds.</param>
    public static void SaveWorldData(Dictionary<string, List<WorldPlace>> worlds)
    {
        string json = JsonSerializer.Serialize(worlds, SerializerOptions);
        File.WriteAllText(Variables.SavePath, json);
    }
    
    /// <summary>
    /// Append world data to the JSON file.
    /// </summary>
    /// <param name="worldName">Name of the world.</param>
    /// <param name="worldPlace">Information about the place that is located in the world.</param>
    /// <returns>Updated dictionary of worlds with information just added.</returns>
    public static Dictionary<string, List<WorldPlace>> AppendWorldData(string worldName, WorldPlace worldPlace)
    {
        if (!Directory.Exists(Path.GetDirectoryName(Variables.SavePath)))
        {
            Directory.CreateDirectory(
                Path.GetDirectoryName(Variables.SavePath) 
                ?? throw new InvalidOperationException("Directory path is null or empty."));
        }

        Dictionary<string, List<WorldPlace>> worlds;
        
        if (File.Exists(Variables.SavePath))
        {
            worlds = JsonSerializer.Deserialize<Dictionary<string, List<WorldPlace>>>
                         (File.ReadAllText(Variables.SavePath), SerializerOptions)
                     ?? new Dictionary<string, List<WorldPlace>>();

            if (worlds.TryGetValue(worldName, out List<WorldPlace>? worldPlaces))
            {
                if (!worldPlaces.Contains(worldPlace))
                    worldPlaces.Add(worldPlace);
            }
            else
            {
                worlds[worldName] = [worldPlace];
            }
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

    /// <summary>
    /// Load world data from the JSON file.
    /// </summary>
    /// <returns>Loaded data about worlds as a dictionary.</returns>
    public static Dictionary<string, List<WorldPlace>> LoadWorldData()
    {
        if (!Directory.Exists(Path.GetDirectoryName(Variables.SavePath)))
        {
            Directory.CreateDirectory(
                Path.GetDirectoryName(Variables.SavePath)
                ?? throw new InvalidOperationException("Directory path is null or empty."));
        }

        if (!File.Exists(Variables.SavePath))
            return new Dictionary<string, List<WorldPlace>>();

        Dictionary<string, List<WorldPlace>> worlds =
            JsonSerializer.Deserialize<Dictionary<string, List<WorldPlace>>>
                (File.ReadAllText(Variables.SavePath), SerializerOptions) 
            ?? throw new InvalidOperationException();

        return worlds;
    }
}