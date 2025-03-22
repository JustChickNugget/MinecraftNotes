using MinecraftNotes.Utilities;
using MinecraftNotes.Structs;
using System.Windows;
using System.Windows.Input;

namespace MinecraftNotes.Windows;

public partial class EditWorldWindow
{
    private Dictionary<string, List<WorldPlace>>? Worlds { get; }
    private string CurrentWorldName { get; }
    
    public EditWorldWindow(string currentWorldName)
    {
        InitializeComponent();

        Worlds = JsonUtilities.LoadWorldData();
        CurrentWorldName = currentWorldName;
        
        NewWorldNameTextBox.Text = currentWorldName;
    }

    // Main events
    
    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        // Edit chosen world.
        
        try
        {
            string newWorldName = NewWorldNameTextBox.Text.Trim();
            
            if (string.IsNullOrEmpty(newWorldName))
                throw new ArgumentNullException(null, "New world name cannot be null or empty.");

            if (Worlds == null)
                throw new ArgumentNullException(null, "Got null trying to get worlds.");
            
            if (Worlds.ContainsKey(newWorldName))
                throw new ArgumentException("The new world name is already in use.");

            if (Worlds.Remove(CurrentWorldName, out List<WorldPlace>? worldPlaces))
            {
                Worlds[newWorldName] = worldPlaces;
                JsonUtilities.SaveWorldData(Worlds);
                
                Close();
            }
            else
            {
                throw new ArgumentNullException(null, "Got null trying to remove or get world values.");
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
        // If user clicks enter, then the button that edits chosen world will be clicked.

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