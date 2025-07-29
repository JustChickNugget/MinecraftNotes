using MinecraftNotes.Windows;
using MinecraftNotes.Utilities;
using MinecraftNotes.Structs;
using MinecraftNotes.Structs.API;
using MinecraftNotes.Other;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
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
            string worldName = WorldNameTextBox.Text.Trim();

            if (string.IsNullOrEmpty(worldName))
                throw new ArgumentNullException(null, "World name cannot be null or empty.");

            string placeName = PlaceNameTextBox.Text.Trim();

            if (string.IsNullOrEmpty(placeName))
                throw new ArgumentNullException(null, "Place name cannot be null or empty.");

            string placeLocationX = PlaceLocationXTextBox.Text.Trim();
            string placeLocationY = PlaceLocationYTextBox.Text.Trim();
            string placeLocationZ = PlaceLocationZTextBox.Text.Trim();

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
            RefreshMenuItem_Click(this, new RoutedEventArgs());
        }
        catch (Exception ex)
        {
            ToolBox.PrintException(ex);
        }
    }

    private void RefreshMenuItem_Click(object sender, RoutedEventArgs e)
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
            ToolBox.PrintException(ex);
        }
    }

    private void WorldExtractMenuItem_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (WorldListBox.SelectedItems.Count <= 0)
                return;
            
            string worldName = (string)WorldListBox.SelectedItem;
            WorldNameTextBox.Text = worldName;
        }
        catch (Exception ex)
        {
            ToolBox.PrintException(ex);
        }
    }
    
    private void WorldEditMenuItem_Click(object sender, RoutedEventArgs e)
    {
        // Launch edit window for worlds.
        
        try
        {
            if (Worlds == null || WorldListBox.SelectedItems.Count <= 0)
                return;

            string worldName = (string)WorldListBox.SelectedItem;
            
            EditWorldWindow editWorldWindow = new(worldName);
            editWorldWindow.ShowDialog();
            
            RefreshMenuItem_Click(this, new RoutedEventArgs());
        }
        catch (Exception ex)
        {
            ToolBox.PrintException(ex);
        }
    }
    
    private void WorldDeleteMenuItem_Click(object sender, RoutedEventArgs e)
    {
        // Delete world from the JSON file.

        try
        {
            if (Worlds == null || WorldListBox.SelectedItems.Count <= 0)
                return;

            foreach (object selectedItem in WorldListBox.SelectedItems)
            {
                string worldName = (string)selectedItem;
                Worlds.Remove(worldName);
            }

            JsonUtilities.SaveWorldData(Worlds);
            RefreshMenuItem_Click(this, new RoutedEventArgs());
        }
        catch (Exception ex)
        {
            ToolBox.PrintException(ex);
        }
    }

    private void PlaceExtractWithWorldNameMenuItem_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (WorldListBox.SelectedItems.Count <= 0 || PlaceListView.SelectedItems.Count <= 0)
                return;
            
            string worldName = (string)WorldListBox.SelectedItem;
            WorldPlace worldPlace = (WorldPlace)PlaceListView.SelectedItem;
            
            WorldNameTextBox.Text = worldName;
            PlaceNameTextBox.Text = worldPlace.Name;
            PlaceLocationXTextBox.Text = worldPlace.Location.X.ToString();
            PlaceLocationYTextBox.Text = worldPlace.Location.Y.ToString();
            PlaceLocationZTextBox.Text = worldPlace.Location.Z.ToString();
        }
        catch (Exception ex)
        {
            ToolBox.PrintException(ex);
        }
    }
    
    private void PlaceExtractWithoutWorldNameMenuItem_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (PlaceListView.SelectedItems.Count <= 0)
                return;
            
            WorldPlace worldPlace = (WorldPlace)PlaceListView.SelectedItem;
            
            PlaceNameTextBox.Text = worldPlace.Name;
            PlaceLocationXTextBox.Text = worldPlace.Location.X.ToString();
            PlaceLocationYTextBox.Text = worldPlace.Location.Y.ToString();
            PlaceLocationZTextBox.Text = worldPlace.Location.Z.ToString();
        }
        catch (Exception ex)
        {
            ToolBox.PrintException(ex);
        }
    }
    
    private void PlaceEditMenuItem_Click(object sender, RoutedEventArgs e)
    {
        // Launch edit window for places.
        
        try
        {
            if (Worlds == null || WorldListBox.SelectedItems.Count <= 0 || PlaceListView.SelectedItems.Count <= 0)
                return;

            string worldName = (string)WorldListBox.SelectedItem;
            WorldPlace worldPlace = (WorldPlace)PlaceListView.SelectedItem;
            
            EditPlaceWindow editPlaceWindow = new(worldName, worldPlace);
            editPlaceWindow.ShowDialog();
            
            RefreshMenuItem_Click(this, new RoutedEventArgs());
        }
        catch (Exception ex)
        {
            ToolBox.PrintException(ex);
        }
    }
    
    private void PlaceDeleteMenuItem_Click(object sender, RoutedEventArgs e)
    {
        // Delete place from the world object in the JSON file.

        try
        {
            if (Worlds == null || WorldListBox.SelectedItems.Count <= 0 || PlaceListView.SelectedItems.Count <= 0)
                return;

            foreach (object selectedItem in PlaceListView.SelectedItems)
            {
                WorldPlace worldPlace = (WorldPlace)
                    (selectedItem ?? throw new InvalidOperationException("Got null while getting data from list view."));

                string worldName = (string)WorldListBox.SelectedItem;
                List<WorldPlace> worldPlaces = Worlds[worldName];

                worldPlaces.Remove(worldPlace);
                Worlds[worldName] = worldPlaces;
            }

            JsonUtilities.SaveWorldData(Worlds);
            RefreshMenuItem_Click(this, new RoutedEventArgs());
        }
        catch (Exception ex)
        {
            ToolBox.PrintException(ex);
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
                PlaceListView.Items.Add(worldPlace);
        }
        catch (Exception ex)
        {
            ToolBox.PrintException(ex);
        }
    }

    private void ViewSaveDirectoryMenuItem_Click(object sender, RoutedEventArgs e)
    {
        // Open directory that contains JSON save file.

        try
        {
            if (!Directory.Exists(Path.GetDirectoryName(Variables.SavePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Variables.SavePath)
                                          ?? throw new InvalidOperationException("Directory path is null or empty."));
            }

            Process.Start(Variables.SavePathProcessStartInfo);
        }
        catch (Exception ex)
        {
            ToolBox.PrintException(ex);
        }
    }

    private void ApplicationAboutMenuItem_Click(object sender, RoutedEventArgs e)
    {
        // Shows information about the application.

        try
        {
            AboutWindow aboutWindow = new();
            aboutWindow.ShowDialog();
        }
        catch (Exception ex)
        {
            ToolBox.PrintException(ex);
        }
    }

    private async void ApplicationCheckForUpdatesMenuItem_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Add("User-Agent", "MinecraftNotes");
            
            string response = await client.GetStringAsync
                ("https://api.github.com/repos/JustChickNugget/MinecraftNotes/releases/latest");

            GitHubApiResponse gitHubApiResponse = JsonSerializer.Deserialize<GitHubApiResponse>(response);

            Version? currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
            Version latestReleaseVersion = Version.Parse(gitHubApiResponse.TagName + ".0");

            if (latestReleaseVersion.CompareTo(currentVersion) >= 1)
            {
                if (MessageBox.Show("An update is available. Would you like to download the update?", "Update checker",
                        MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = Variables.RepositoryLink,
                        UseShellExecute = true
                    });
                }
            }
            else
            {
                MessageBox.Show("Current version is up to date.", "Update checker",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        catch (Exception ex)
        {
            ToolBox.PrintException(ex);
        }
    }

    private void DeveloperGitHubMenuItem_Click(object sender, RoutedEventArgs e)
    {
        // Open developer's GitHub profile page.

        try
        {
            Process.Start(Variables.DeveloperGitHubProcessStartInfo);
        }
        catch (Exception ex)
        {
            ToolBox.PrintException(ex);
        }
    }

    // Other events

    private void Window_OnLoaded(object sender, RoutedEventArgs e)
    {
        // Get JSON data on application load.

        try
        {
            RefreshMenuItem_Click(this, new RoutedEventArgs());
        }
        catch (Exception ex)
        {
            ToolBox.PrintException(ex);
        }
    }

    private void Window_OnKeyDown(object sender, KeyEventArgs e)
    {
        // If user clicks enter, then the button that adds world to the JSON file will be clicked.

        try
        {
            if (e.Key != Key.Enter)
                return;

            e.Handled = true;
            WorldAddButton_Click(this, new RoutedEventArgs());
        }
        catch (Exception ex)
        {
            ToolBox.PrintException(ex);
        }
    }
}