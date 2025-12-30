using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using MinecraftNotes.Models.Minecraft;
using MinecraftNotes.Other;
using MinecraftNotes.Utilities;
using MinecraftNotes.ViewModels;
using MinecraftNotes.Windows;
using Location = MinecraftNotes.Models.Minecraft.Location;

namespace MinecraftNotes;

/// <summary>
/// Main window of the application.
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>
    /// View model of the main window.
    /// </summary>
    private MainViewModel MainViewModel { get; } = new();

    /// <summary>
    /// A constructor of the main window.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
        DataContext = MainViewModel;
    }

    #region Main events

    /// <summary>
    /// Add world to the JSON data file using input data.
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="e">Event arguments</param>
    /// <exception cref="ArgumentNullException">There are null values</exception>
    /// <exception cref="FormatException">String values can't be parsed to an integer</exception>
    private async void AddWorldButton_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            string worldName = (WorldNameTextBox.Text ?? "").Trim();

            if (string.IsNullOrEmpty(worldName))
            {
                throw new ArgumentNullException(null, "World name is null or empty.");
            }

            string placeName = (PlaceNameTextBox.Text ?? "").Trim();

            if (string.IsNullOrEmpty(placeName))
            {
                throw new ArgumentNullException(null, "Place name is null or empty.");
            }

            string placeLocationX = string.IsNullOrEmpty(PlaceLocationXTextBox.Text)
                ? "0"
                : PlaceLocationXTextBox.Text.Trim();

            string placeLocationY = string.IsNullOrEmpty(PlaceLocationYTextBox.Text)
                ? "0"
                : PlaceLocationYTextBox.Text.Trim();

            string placeLocationZ = string.IsNullOrEmpty(PlaceLocationZTextBox.Text)
                ? "0"
                : PlaceLocationZTextBox.Text.Trim();

            if (!int.TryParse(placeLocationX, out int placeLocationXInteger))
            {
                throw new FormatException("X coordinate of the place location could not be parsed.");
            }

            if (!int.TryParse(placeLocationY, out int placeLocationYInteger))
            {
                throw new FormatException("Y coordinate of the place location could not be parsed.");
            }

            if (!int.TryParse(placeLocationZ, out int placeLocationZInteger))
            {
                throw new FormatException("Z coordinate of the place location could not be parsed.");
            }

            Location location = new()
            {
                X = placeLocationXInteger,
                Y = placeLocationYInteger,
                Z = placeLocationZInteger
            };

            WorldPlace worldPlace = new()
            {
                Name = placeName,
                Location = location
            };

            MainViewModel.Worlds = JsonUtilities.AppendWorldData(worldName, worldPlace);
        }
        catch (Exception exception)
        {
            await ToolBox.PrintException(this, exception, nameof(MainWindow), nameof(AddWorldButton_OnClick));
        }
    }

    /// <summary>
    /// Refresh data.
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="e">Event arguments</param>
    private async void RefreshMenuItem_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            MainViewModel.Worlds = JsonUtilities.LoadWorldData();
        }
        catch (Exception exception)
        {
            await ToolBox.PrintException(this, exception, nameof(MainWindow), nameof(RefreshMenuItem_OnClick));
        }
    }

    #region World controls

    /// <summary>
    /// Extract data from the selected world into the text box.
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="e">Event arguments</param>
    private async void WorldExtractMenuItem_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            string worldName = (string)(WorldListBox.SelectedItem ?? "");

            if (string.IsNullOrEmpty(worldName))
            {
                return;
            }

            WorldNameTextBox.Text = worldName;
        }
        catch (Exception exception)
        {
            await ToolBox.PrintException(this, exception, nameof(MainWindow), nameof(WorldExtractMenuItem_OnClick));
        }
    }

    /// <summary>
    /// Edit selected world.
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="e">Event arguments</param>
    private async void WorldEditMenuItem_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (MainViewModel.Worlds == null || MainViewModel.Worlds.Count <= 0)
            {
                return;
            }

            string worldName = (string)(WorldListBox.SelectedItem ?? "");

            if (string.IsNullOrEmpty(worldName))
            {
                return;
            }

            EditWorldWindow editWorldWindow = new(MainViewModel.Worlds, worldName);
            await editWorldWindow.ShowDialog(this);

            MainViewModel.RefreshWorldData();
        }
        catch (Exception exception)
        {
            await ToolBox.PrintException(this, exception, nameof(MainWindow), nameof(WorldEditMenuItem_OnClick));
        }
    }

    /// <summary>
    /// Delete selected world.
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="e">Event arguments</param>
    private async void WorldDeleteMenuItem_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (MainViewModel.Worlds == null || MainViewModel.Worlds.Count <= 0)
            {
                return;
            }

            string worldName = (string)(WorldListBox.SelectedItem ?? "");

            if (string.IsNullOrEmpty(worldName))
            {
                return;
            }

            MainViewModel.Worlds.Remove(worldName);
            JsonUtilities.SaveWorldData(MainViewModel.Worlds);

            MainViewModel.RefreshWorldData();
        }
        catch (Exception exception)
        {
            await ToolBox.PrintException(this, exception, nameof(MainWindow), nameof(WorldDeleteMenuItem_OnClick));
        }
    }

    #endregion

    #region Place controls

    /// <summary>
    /// Extract data from the selected place (with the world name) into text boxes.
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="e">Event arguments</param>
    private async void PlaceDataGridExtractWithWorldNameMenuItem_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (PlaceDataGrid.SelectedItem == null)
            {
                return;
            }

            string worldName = (string)(WorldListBox.SelectedItem ?? "");

            if (string.IsNullOrEmpty(worldName))
            {
                return;
            }

            WorldPlace worldPlace = (WorldPlace)PlaceDataGrid.SelectedItem;

            WorldNameTextBox.Text = worldName;
            PlaceNameTextBox.Text = worldPlace.Name;
            PlaceLocationXTextBox.Text = worldPlace.Location.X.ToString();
            PlaceLocationYTextBox.Text = worldPlace.Location.Y.ToString();
            PlaceLocationZTextBox.Text = worldPlace.Location.Z.ToString();
        }
        catch (Exception exception)
        {
            await ToolBox.PrintException(
                this,
                exception,
                nameof(MainWindow),
                nameof(PlaceDataGridExtractWithWorldNameMenuItem_OnClick));
        }
    }

    /// <summary>
    /// Extract data from the selected place (except the world name) into text boxes.
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="e">Event arguments</param>
    private async void PlaceDataGridExtractWithoutWorldNameMenuItem_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (PlaceDataGrid.SelectedItem == null)
            {
                return;
            }

            WorldPlace worldPlace = (WorldPlace)PlaceDataGrid.SelectedItem;

            PlaceNameTextBox.Text = worldPlace.Name;
            PlaceLocationXTextBox.Text = worldPlace.Location.X.ToString();
            PlaceLocationYTextBox.Text = worldPlace.Location.Y.ToString();
            PlaceLocationZTextBox.Text = worldPlace.Location.Z.ToString();
        }
        catch (Exception exception)
        {
            await ToolBox.PrintException(
                this,
                exception,
                nameof(MainWindow),
                nameof(PlaceDataGridExtractWithoutWorldNameMenuItem_OnClick));
        }
    }

    /// <summary>
    /// Edit selected place.
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="e">Event arguments</param>
    private async void PlaceDataGridEditMenuItem_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (MainViewModel.Worlds == null || MainViewModel.Worlds.Count <= 0 || PlaceDataGrid.SelectedItem == null)
            {
                return;
            }

            string worldName = (string)(WorldListBox.SelectedItem ?? "");

            if (string.IsNullOrEmpty(worldName))
            {
                throw new ArgumentNullException(null, "World is not selected.");
            }

            WorldPlace worldPlace = (WorldPlace)PlaceDataGrid.SelectedItem;

            EditPlaceWindow editPlaceWindow = new(MainViewModel.Worlds, worldName, worldPlace);
            await editPlaceWindow.ShowDialog(this);

            MainViewModel.RefreshWorldData();
        }
        catch (Exception exception)
        {
            await ToolBox.PrintException(
                this,
                exception,
                nameof(MainWindow),
                nameof(PlaceDataGridEditMenuItem_OnClick));
        }
    }

    /// <summary>
    /// Delete selected place.
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="e">Event arguments</param>
    private async void PlaceDataGridDeleteMenuItem_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (MainViewModel.Worlds == null || MainViewModel.Worlds.Count <= 0 || PlaceDataGrid.SelectedItem == null)
            {
                return;
            }

            string worldName = (string)(WorldListBox.SelectedItem ?? "");

            if (string.IsNullOrEmpty(worldName))
            {
                throw new ArgumentNullException(null, "World is not selected.");
            }

            WorldPlace worldPlace = (WorldPlace)PlaceDataGrid.SelectedItem;

            List<WorldPlace> worldPlaces = MainViewModel.Worlds[worldName];
            worldPlaces.Remove(worldPlace);

            MainViewModel.Worlds[worldName] = worldPlaces;
            JsonUtilities.SaveWorldData(MainViewModel.Worlds);

            MainViewModel.RefreshWorldData();
        }
        catch (Exception exception)
        {
            await ToolBox.PrintException(
                this,
                exception,
                nameof(MainWindow),
                nameof(PlaceDataGridDeleteMenuItem_OnClick));
        }
    }

    #endregion

    #endregion

    #region Menu events

    /// <summary>
    /// View save directory of the JSON data file.
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="e">Event arguments</param>
    /// <exception cref="ArgumentNullException">There are null values</exception>
    private async void ViewSaveDirectoryMenuItem_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            string? savePathDirectory = Path.GetDirectoryName(Constants.SavePath);

            if (string.IsNullOrEmpty(savePathDirectory))
            {
                throw new ArgumentNullException(null, "Save directory path is null or empty.");
            }

            if (!Directory.Exists(savePathDirectory))
            {
                Directory.CreateDirectory(savePathDirectory);
            }

            Process.Start(Constants.SavePathProcessStartInfo);
        }
        catch (Exception exception)
        {
            await ToolBox.PrintException(
                this,
                exception,
                nameof(MainWindow),
                nameof(ViewSaveDirectoryMenuItem_OnClick));
        }
    }

    /// <summary>
    /// Show "About" window.
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="e">Event arguments</param>
    private async void AboutApplicationMenuItem_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            AboutWindow aboutWindow = new();
            await aboutWindow.ShowDialog(this);
        }
        catch (Exception exception)
        {
            await ToolBox.PrintException(this, exception, nameof(MainWindow), nameof(AboutApplicationMenuItem_OnClick));
        }
    }

    /// <summary>
    /// Check for updates.
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="e">Event arguments</param>
    private async void CheckForUpdatesMenuItem_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            CheckUpdatesWindow checkUpdatesWindow = new();
            await checkUpdatesWindow.ShowDialog(this);
        }
        catch (Exception exception)
        {
            await ToolBox.PrintException(this, exception, nameof(MainWindow), nameof(CheckForUpdatesMenuItem_OnClick));
        }
    }

    #endregion

    #region Window events

    /// <summary>
    /// Handle window's load.
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="e">Event arguments</param>
    private async void Window_OnLoaded(object? sender, RoutedEventArgs e)
    {
        try
        {
            Dictionary<string, List<WorldPlace>> worlds = JsonUtilities.LoadWorldData();
            MainViewModel.Worlds = worlds;
        }
        catch (Exception exception)
        {
            await ToolBox.PrintException(this, exception, nameof(MainWindow), nameof(Window_OnLoaded));
        }
    }

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
            AddWorldButton_OnClick(sender, e);
        }
        catch (Exception exception)
        {
            await ToolBox.PrintException(this, exception, nameof(MainWindow), nameof(Window_OnKeyDown));
        }
    }

    #endregion
}