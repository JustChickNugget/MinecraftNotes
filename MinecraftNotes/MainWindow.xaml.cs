using MinecraftNotes.Utilities;
using MinecraftNotes.Structs;
using MinecraftNotes.Other;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MinecraftNotes;

public partial class MainWindow
{
    private Dictionary<string, List<WorldPlace>>? Worlds { get; set; }
    
    public MainWindow()
    {
        InitializeComponent();
    }
    
    private void WorldAddButton_Click(object sender, RoutedEventArgs e)
    {
        // Add world to the JSON file using data from the inputs.
        
        try
        {
            string worldName = WorldNameTextBox.Text;

            if (string.IsNullOrEmpty(worldName))
                throw new ArgumentNullException(null, "World name cannot be null or empty.");
        
            string placeName = PlaceNameTextBox.Text;
            
            if (string.IsNullOrEmpty(placeName))
                throw new ArgumentNullException(null, "Place name cannot be null or empty.");
            
            string placeLocationX = PlaceLocationXTextBox.Text;
            string placeLocationY = PlaceLocationYTextBox.Text;
            string placeLocationZ = PlaceLocationZTextBox.Text;

            if (string.IsNullOrEmpty(placeLocationX) && string.IsNullOrEmpty(placeLocationY) && string.IsNullOrEmpty(placeLocationZ))
                throw new ArgumentNullException(null, "Place location cannot be null or empty.");
            
            Location location = new()
            {
                X = string.IsNullOrEmpty(placeLocationX) ? 0 : int.Parse(placeLocationX),
                Y = string.IsNullOrEmpty(placeLocationY) ? 0 : int.Parse(placeLocationY),
                Z = string.IsNullOrEmpty(placeLocationZ) ? 0 : int.Parse(placeLocationZ)
            };

            WorldPlace worldPlace = new()
            {
                Name = placeName,
                Location = location
            };

            Worlds = JsonUtilities.AppendWorldData(worldName, worldPlace);
            WorldRefreshMenuItem_Click(this, new RoutedEventArgs());
        }
        catch (Exception ex)
        {
            PrintException(ex);
        }
    }

    private void WorldRefreshMenuItem_Click(object sender, RoutedEventArgs e)
    {
        // Refresh worlds data from the JSON file.
        
        try
        {
            Worlds = JsonUtilities.LoadWorldData();
            string selectedWorldName = "";
            
            if (WorldListBox.SelectedItems.Count > 0)
                selectedWorldName = (string)WorldListBox.SelectedItem;
            
            WorldListBox.Items.Clear();
            
            foreach (string worldName in Worlds.Keys)
                WorldListBox.Items.Add(worldName);

            if (!string.IsNullOrEmpty(selectedWorldName) && WorldListBox.Items.Contains(selectedWorldName))
                WorldListBox.SelectedItem = selectedWorldName;
        }
        catch (Exception ex)
        {
            PrintException(ex);
        }
    }

    private void WorldDeleteMenuItem_Click(object sender, RoutedEventArgs e)
    {
        // Delete world from the JSON file.
        
        try
        {
            if (Worlds == null || WorldListBox.SelectedItems.Count <= 0)
                return;
            
            Worlds.Remove((string)WorldListBox.SelectedItem);
            JsonUtilities.SaveWorldData(Worlds);
            
            WorldRefreshMenuItem_Click(this, new RoutedEventArgs());
        }
        catch (Exception ex)
        {
            PrintException(ex);
        }
    }

    private void PlaceDeleteMenuItem_Click(object sender, RoutedEventArgs e)
    {
        // Delete place from the world object in the JSON file.
        
        try
        {
            if (Worlds == null || WorldListBox.SelectedItems.Count <= 0)
                return;

            foreach (object selectedItem in PlaceListView.SelectedItems)
            {
                WorldPlace worldPlace =
                    (WorldPlace)(selectedItem ?? throw new InvalidOperationException("Got null while getting data from list view."));

                string worldName = (string)WorldListBox.SelectedItem;
                List<WorldPlace> worldPlaces = Worlds[worldName];
            
                worldPlaces.Remove(worldPlace);
                Worlds[worldName] = worldPlaces;
            }
            
            JsonUtilities.SaveWorldData(Worlds);
            WorldRefreshMenuItem_Click(this, new RoutedEventArgs());
        }
        catch (Exception ex)
        {
            PrintException(ex);
        }
    }
    
    private void WorldsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        /*
         * If user selected other world in the list box, we update the place list view to print
         * all the places of the selected world.
         */
        
        try
        {
            string selectedItem = (string)WorldListBox.SelectedItem;

            if (string.IsNullOrEmpty(selectedItem))
            {
                PlaceListView.Items.Clear();
                return;
            }

            if (Worlds == null)
                return;
            
            PlaceListView.Items.Clear();
            
            foreach (WorldPlace worldPlace in Worlds[selectedItem])
            {
                PlaceListView.Items.Add(new WorldPlace
                {
                    Name = worldPlace.Name,
                    Location = worldPlace.Location
                });
            }
        }
        catch (Exception ex)
        {
            PrintException(ex);
        }
    }
    
    private void ViewSaveDirectoryMenuItem_Click(object sender, RoutedEventArgs e)
    {
        // Open directory that contains JSON save file.
        
        try
        {
            if (!Directory.Exists(Path.GetDirectoryName(Variables.SavePath)))
            {
                Directory.CreateDirectory
                    (Path.GetDirectoryName(Variables.SavePath) ?? throw new InvalidOperationException("Directory path is null or empty."));
            }
            
            Process.Start(Variables.SavePathProcessStartInfo);
        }
        catch (Exception ex)
        {
            PrintException(ex);
        }
    }
    
    private void ApplicationInformationMenuItem_Click(object sender, RoutedEventArgs e)
    {
        // Shows information about the application.
        
        try
        {
            MessageBox.Show(
                $"""
                Minecraft Notes ({Assembly.GetExecutingAssembly().GetName().Version})
                Developer: JustChickNugget (2025)
                
                - Take notes about your Minecraft worlds
                """,
                "Application information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            PrintException(ex);
        }
    }
    
    private void DeveloperGitHubMenuItem_Click(object sender, RoutedEventArgs e)
    {
        // Open developer's GitHub profile page.
        
        try
        {
            Process.Start(Variables.GitHubProcessStartInfo);
        }
        catch (Exception ex)
        {
            PrintException(ex);
        }
    }
    
    // Other events
    
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        // Get JSON data on application load.
        
        try
        {
            WorldRefreshMenuItem_Click(this, new RoutedEventArgs());
        }
        catch (Exception ex)
        {
            PrintException(ex);
        }
    }
    
    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        try
        {
            if (e.Key != Key.Enter)
                return;
            
            e.Handled = true;
            WorldAddButton_Click(this, new RoutedEventArgs());
        }
        catch (Exception ex)
        {
            PrintException(ex);
        }
    }
    
    // Other functions
    
    private static void PrintException(Exception ex)
    {
        // Print exception if it has occurred.
        
#if DEBUG
        MessageBox.Show(ex.ToString(), $"An exception has occurred ({ex.GetType().FullName})", MessageBoxButton.OK, MessageBoxImage.Error);
#else
        MessageBox.Show(ex.Message, $"An exception has occurred ({ex.GetType().FullName})", MessageBoxButton.OK, MessageBoxImage.Error);
#endif
    }
}