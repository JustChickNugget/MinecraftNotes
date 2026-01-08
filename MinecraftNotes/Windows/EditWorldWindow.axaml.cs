using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using MinecraftNotes.Models.Minecraft;
using MinecraftNotes.Utilities;
using MinecraftNotes.Utilities.Minecraft;

namespace MinecraftNotes.Windows;

/// <summary>
/// A window that lets user edit a world.
/// </summary>
public partial class EditWorldWindow : Window
{
    /// <summary>
    /// Dictionary of world data.
    /// </summary>
    private Dictionary<string, List<WorldPlace>>? Worlds { get; }

    /// <summary>
    /// Old world name.
    /// </summary>
    private string OldWorldName { get; }

    /// <summary>
    /// A constructor of the edit world window.
    /// </summary>
    /// <param name="worlds">Dictionary of worlds</param>
    /// <param name="oldWorldName">Old world name</param>
    public EditWorldWindow(Dictionary<string, List<WorldPlace>> worlds, string oldWorldName)
    {
        InitializeComponent();

        Worlds = worlds;
        OldWorldName = oldWorldName;

        NewWorldNameTextBox.Text = OldWorldName;
    }

    #region Main events

    /// <summary>
    /// Save edited world.
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

            string newWorldName = (NewWorldNameTextBox.Text ?? "").Trim();

            if (string.IsNullOrWhiteSpace(newWorldName))
            {
                throw new ArgumentNullException(null, "New world name is null or empty.");
            }

            if (Worlds.ContainsKey(newWorldName))
            {
                throw new ArgumentException("World name is already in use.");
            }

            if (Worlds.Remove(OldWorldName, out List<WorldPlace>? worldPlaces))
            {
                Worlds[newWorldName] = worldPlaces;
                DataLoader.SaveWorldData(Worlds);

                Close();
            }
            else
            {
                throw new ArgumentNullException(null, "Unable to remove old world.");
            }
        }
        catch (Exception exception)
        {
            await ToolBox.PrintException(this, exception, nameof(EditWorldWindow), nameof(SaveButton_OnClick));
        }
    }

    #endregion

    #region Window events

    /// <summary>
    /// Handle user's input.
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
            await ToolBox.PrintException(this, exception, nameof(EditWorldWindow), nameof(Window_OnKeyDown));
        }
    }

    #endregion
}