using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using MinecraftNotes.Models.Minecraft;
using MinecraftNotes.Utilities.Minecraft;
using Location = MinecraftNotes.Models.Minecraft.Location;

namespace MinecraftNotes.Windows;

/// <summary>
/// A window that lets user edit a place.
/// </summary>
public partial class EditPlaceWindow : Window
{
    /// <summary>
    /// Dictionary of world data.
    /// </summary>
    private Dictionary<string, List<WorldPlace>>? Worlds { get; }

    /// <summary>
    /// Selected world name.
    /// </summary>
    private string WorldName { get; }

    /// <summary>
    /// Old world place data.
    /// </summary>
    private WorldPlace OldWorldPlace { get; }

    /// <summary>
    /// A constructor of the edit place window.
    /// </summary>
    /// <param name="worlds">Dictionary of worlds</param>
    /// <param name="worldName">Selected world name</param>
    /// <param name="oldWorldPlace">Old world place data</param>
    public EditPlaceWindow(Dictionary<string, List<WorldPlace>> worlds, string worldName, WorldPlace oldWorldPlace)
    {
        InitializeComponent();

        Worlds = worlds;
        WorldName = worldName;
        OldWorldPlace = oldWorldPlace;

        NewPlaceNameTextBox.Text = OldWorldPlace.Name;
        NewPlaceLocationXTextBox.Text = OldWorldPlace.Location.X.ToString();
        NewPlaceLocationYTextBox.Text = OldWorldPlace.Location.Y.ToString();
        NewPlaceLocationZTextBox.Text = OldWorldPlace.Location.Z.ToString();
    }

    #region Main events

    /// <summary>
    /// Save edited place.
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="e">Event arguments</param>
    private async void SaveButton_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (Worlds == null)
            {
                throw new ArgumentNullException(null, "Got null trying to get worlds.");
            }

            string newPlaceName = (NewPlaceNameTextBox.Text ?? "").Trim();

            if (string.IsNullOrEmpty(newPlaceName))
            {
                throw new ArgumentNullException(null, "New place name is null or empty.");
            }

            string newPlaceLocationX = (NewPlaceLocationXTextBox.Text ?? "").Trim();
            string newPlaceLocationY = (NewPlaceLocationYTextBox.Text ?? "").Trim();
            string newPlaceLocationZ = (NewPlaceLocationZTextBox.Text ?? "").Trim();

            Location newLocation = new()
            {
                X = string.IsNullOrEmpty(newPlaceLocationX) ? 0 : int.Parse(newPlaceLocationX),
                Y = string.IsNullOrEmpty(newPlaceLocationY) ? 0 : int.Parse(newPlaceLocationY),
                Z = string.IsNullOrEmpty(newPlaceLocationZ) ? 0 : int.Parse(newPlaceLocationZ)
            };

            WorldPlace newWorldPlace = new()
            {
                Name = newPlaceName,
                Location = newLocation
            };

            if (!Worlds.TryGetValue(WorldName, out List<WorldPlace>? worldPlaces))
            {
                throw new ArgumentException("Got null trying to get world places.");
            }

            if (worldPlaces.Contains(newWorldPlace))
            {
                throw new ArgumentException("World place is already in use.");
            }

            int oldWorldPlaceIndex = worldPlaces.FindIndex(foundWorldPlace => foundWorldPlace == OldWorldPlace);
            worldPlaces[oldWorldPlaceIndex] = newWorldPlace;

            Worlds[WorldName] = worldPlaces;
            DataLoader.SaveWorldData(Worlds);

            Close();
        }
        catch (Exception exception)
        {
            await NuggetLib.Views.Services.ExceptionHandleService.ShowExceptionAsync(
                this,
                exception,
                nameof(EditPlaceWindow),
                nameof(SaveButton_OnClick));
        }
    }

    #endregion

    #region Window events

    /// <summary>
    /// Handle user input.
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="e">Event arguments</param>
    private async void Window_OnKeyDown(object? sender, KeyEventArgs e)
    {
        try
        {
            if (e.Key != Key.Enter)
            {
                return;
            }

            e.Handled = true;
            SaveButton_OnClick(sender, e);
        }
        catch (Exception exception)
        {
            await NuggetLib.Views.Services.ExceptionHandleService.ShowExceptionAsync(
                this,
                exception,
                nameof(EditPlaceWindow),
                nameof(Window_OnKeyDown));
        }
    }

    #endregion
}