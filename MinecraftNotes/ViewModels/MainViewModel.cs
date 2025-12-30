using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MinecraftNotes.Models.Minecraft;

namespace MinecraftNotes.ViewModels;

/// <summary>
/// A view model for controlling world list data.
/// </summary>
public sealed class MainViewModel : INotifyPropertyChanged
{
    /// <summary>
    /// Property: dictionary of world data.
    /// </summary>
    public Dictionary<string, List<WorldPlace>>? Worlds
    {
        get => _worlds;
        set
        {
            if (_worlds == value)
            {
                return;
            }

            _worlds = value;
            OnPropertyChanged();
            RefreshWorldData();
        }
    }

    /// <summary>
    /// Property: selected world.
    /// </summary>
    public string? SelectedWorld
    {
        get => _selectedWorld;
        set
        {
            if (_selectedWorld == value)
            {
                return;
            }

            _selectedWorld = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(SelectedWorldPlaces));
        }
    }

    /// <summary>
    /// Property: observable collection of world names.
    /// </summary>
    public ObservableCollection<string> WorldNames => new(Worlds == null ? [] : Worlds.Keys);

    /// <summary>
    /// Property: observable collection of world places of the selected world.
    /// </summary>
    public ObservableCollection<WorldPlace> SelectedWorldPlaces
    {
        get
        {
            if (string.IsNullOrEmpty(SelectedWorld) ||
                Worlds == null ||
                !Worlds.TryGetValue(SelectedWorld, out List<WorldPlace>? worldPlaces))
            {
                return [];
            }

            return new ObservableCollection<WorldPlace>(worldPlaces);
        }
    }

    /// <summary>
    /// Field: dictionary of world data.
    /// </summary>
    private Dictionary<string, List<WorldPlace>>? _worlds;

    /// <summary>
    /// Field: selected world.
    /// </summary>
    private string? _selectedWorld;

    /// <summary>
    /// Property changed event handler.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Handle on property changed event.
    /// </summary>
    /// <param name="propertyName">Property name</param>
    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Refresh world data.
    /// </summary>
    public void RefreshWorldData()
    {
        OnPropertyChanged(nameof(WorldNames));
        OnPropertyChanged(nameof(SelectedWorldPlaces));
    }
}