using MinecraftNotes.Utilities;
using MinecraftNotes.Structs;
using System.Windows;
using System.Windows.Input;

namespace MinecraftNotes.Windows;

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

    // Main events
    
    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        // Edit chosen place.
        
        try
        {
            string newPlaceName = NewPlaceNameTextBox.Text.Trim();
            string newPlaceLocationX = NewPlaceLocationXTextBox.Text.Trim();
            string newPlaceLocationY = NewPlaceLocationYTextBox.Text.Trim();
            string newPlaceLocationZ = NewPlaceLocationZTextBox.Text.Trim();
            
            if (string.IsNullOrEmpty(newPlaceName))
                throw new ArgumentNullException(null, "New place name cannot be null or empty.");
            
            if (string.IsNullOrEmpty(newPlaceLocationX) && string.IsNullOrEmpty(newPlaceLocationY) && string.IsNullOrEmpty(newPlaceLocationZ))
                throw new ArgumentNullException(null, "New place location cannot be null or empty.");
            
            if (Worlds != null)
            {
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
                
                int currentWorldPlaceIndex = worldPlaces.FindIndex(findWorldPlace => findWorldPlace == CurrentWorldPlace);
                worldPlaces[currentWorldPlaceIndex] = newWorldPlace;

                Worlds[CurrentWorldName] = worldPlaces;
                JsonUtilities.SaveWorldData(Worlds);

                Close();
            }
            else
            {
                throw new ArgumentNullException(null, "Got null trying to get world values.");
            }
        }
        catch (Exception ex)
        {
            ToolBox.PrintException(ex);
        }
    }
    
    // Other events

    private void Window_OnKeyDown(object sender, KeyEventArgs e)
    {
        // If user clicks enter, then the button that edits chosen place will be clicked.

        try
        {
            if (e.Key != Key.Enter)
                return;

            e.Handled = true;
            SaveButton_Click(this, new RoutedEventArgs());
        }
        catch (Exception ex)
        {
            ToolBox.PrintException(ex);
        }
    }
}