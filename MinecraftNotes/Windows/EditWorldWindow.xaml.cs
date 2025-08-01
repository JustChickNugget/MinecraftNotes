using MinecraftNotes.Utilities;
using MinecraftNotes.Structs;
using System.Windows;
using System.Windows.Input;

namespace MinecraftNotes.Windows;

/// <summary>
/// A window that allows the user to edit information about the world.
/// </summary>
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

    #region MAIN EVENTS
    
    /// <summary>
    /// Save changes to the world and close the window.
    /// </summary>
    private void SaveButton_OnClick(object sender, RoutedEventArgs e)
    {
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
            SaveButton_OnClick(sender, e);
        }
        catch (Exception ex)
        {
            ToolBox.PrintException(ex);
        }
    }
    
    #endregion
}