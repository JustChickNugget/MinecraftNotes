using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using MinecraftNotes.Models.Minecraft;
using MinecraftNotes.Other;

namespace MinecraftNotes.Utilities.Minecraft;

/// <summary>
/// Static class containing functions to easily work with the JSON and worlds.
/// </summary>
public static class DataLoader
{
    /// <summary>
    /// Default JSON serializer options.
    /// </summary>
    private static JsonSerializerOptions SerializerOptions { get; } = new()
    {
        WriteIndented = true
    };

    /// <summary>
    /// Save world data to the JSON data file.
    /// </summary>
    /// <param name="worlds">Dictionary of worlds</param>
    public static void SaveWorldData(Dictionary<string, List<WorldPlace>> worlds)
    {
        string json = JsonSerializer.Serialize(worlds, SerializerOptions);
        File.WriteAllText(Constants.SavePath, json);
    }

    /// <summary>
    /// Append world data to the JSON data file.
    /// </summary>
    /// <param name="worldName">Name of the world</param>
    /// <param name="worldPlace">Information about the world place</param>
    /// <returns>An updated dictionary of worlds</returns>
    public static Dictionary<string, List<WorldPlace>> AppendWorldData(string worldName, WorldPlace worldPlace)
    {
        if (!Directory.Exists(Path.GetDirectoryName(Constants.SavePath)))
        {
            Directory.CreateDirectory(
                Path.GetDirectoryName(Constants.SavePath) ??
                throw new InvalidOperationException("Directory path is null or empty."));
        }

        Dictionary<string, List<WorldPlace>> worlds;

        if (File.Exists(Constants.SavePath))
        {
            worlds = JsonSerializer.Deserialize<Dictionary<string, List<WorldPlace>>>(
                         File.ReadAllText(Constants.SavePath), SerializerOptions) ??
                     new Dictionary<string, List<WorldPlace>>();

            if (worlds.TryGetValue(worldName, out List<WorldPlace>? worldPlaces))
            {
                if (!worldPlaces.Contains(worldPlace))
                {
                    worldPlaces.Add(worldPlace);
                }
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
        File.WriteAllText(Constants.SavePath, json);

        return worlds;
    }

    /// <summary>
    /// Load world data from the JSON data file.
    /// </summary>
    /// <returns>Dictionary of worlds</returns>
    public static Dictionary<string, List<WorldPlace>> LoadWorldData()
    {
        if (!Directory.Exists(Path.GetDirectoryName(Constants.SavePath)))
        {
            Directory.CreateDirectory(
                Path.GetDirectoryName(Constants.SavePath) ??
                throw new InvalidOperationException("Directory path is null or empty."));
        }

        if (!File.Exists(Constants.SavePath))
        {
            return new Dictionary<string, List<WorldPlace>>();
        }

        Dictionary<string, List<WorldPlace>> worlds = JsonSerializer.Deserialize<Dictionary<string, List<WorldPlace>>>(
            File.ReadAllText(Constants.SavePath), SerializerOptions) ?? throw new InvalidOperationException();

        return worlds;
    }
}