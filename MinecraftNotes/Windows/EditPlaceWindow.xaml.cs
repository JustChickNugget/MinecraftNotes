using MinecraftNotes.Utilities;
using MinecraftNotes.Structs;
using System.Windows;
using System.Windows.Input;

namespace MinecraftNotes.Windows;

/// <summary>
/// A window that allows the user to edit information about the place.
/// </summary>
public partial class EditPlaceWindow
{
    private Dictionary<string, List<WorldPlace>>? Worlds { get; }
    private string CurrentWorldName { get; }
    private WorldPlace CurrentWorldPlace { get; }
    
    public EditPlaceWindow(string currentWorldName, WorldPlace currentWorldPlace)
    {
        InitializeComponent();

        Worlds = JsonUtilities.LoadWorldData();
        CurrentWorldName = currentWorldName;
        CurrentWorldPlace = currentWorldPlace;

        NewPlaceNameTextBox.Text = currentWorldPlace.Name;
        NewPlaceLocationXTextBox.Text = currentWorldPlace.Location.X.ToString();
        NewPlaceLocationYTextBox.Text = currentWorldPlace.Location.Y.ToString();
        NewPlaceLocationZTextBox.Text = currentWorldPlace.Location.Z.ToString();
    }

    #region MAIN EVENTS
    
    /// <summary>
    /// Save changes to the place and close the window.
    /// </summary>
    private void SaveButton_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            string newPlaceName = NewPlaceNameTextBox.Text.Trim();
            string newPlaceLocationX = NewPlaceLocationXTextBox.Text.Trim();
            string newPlaceLocationY = NewPlaceLocationYTextBox.Text.Trim();
            string newPlaceLocationZ = NewPlaceLocationZTextBox.Text.Trim();
            
            if (string.IsNullOrEmpty(newPlaceName))
                throw new ArgumentNullException(null, "New place name cannot be null or empty.");

            if (Worlds == null)
                throw new ArgumentNullException(null, "Got null trying to get world values.");
            
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

            if (!Worlds.TryGetValue(CurrentWorldName, out List<WorldPlace>? worldPlaces))
                throw new ArgumentException("Got null trying to get world values.");

            if (worldPlaces.Contains(newWorldPlace))
                throw new ArgumentException("The new world place is already in use.");

            int currentWorldPlaceIndex =
                worldPlaces.FindIndex(findWorldPlace => findWorldPlace == CurrentWorldPlace);
            
            worldPlaces[currentWorldPlaceIndex] = newWorldPlace;
            Worlds[CurrentWorldName] = worldPlaces;
            
            JsonUtilities.SaveWorldData(Worlds);
            Close();
        }
        catch (Exception ex)
        {
            ToolBox.PrintException(ex);
        }
    }
    
    #endregion
    
    #region WINDOW EVENTS

    /// <summary>
    /// Handle user's input. If the user presses 'Enter', the button will be pressed that saves edits to the JSON file.
    /// </summary>
    private void Window_OnKeyDown(object sender, KeyEventArgs e)
    {
        try
        {
            if (e.Key != Key.Enter)
                return;

            e.Handled = true;
            SaveButton_OnClick(this, new RoutedEventArgs());
        }
        catch (Exception ex)
        {
            ToolBox.PrintException(ex);
        }
    }
    
    #endregion
}