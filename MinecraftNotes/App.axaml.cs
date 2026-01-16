using System;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MinecraftNotes.Other;

namespace MinecraftNotes;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
        MoveDataFileFromOldPath();
    }

    /// <summary>
    /// Move data file from the old path to the new if exists without overwriting the new one.
    /// </summary>
    /// <exception cref="ArgumentNullException">Unable to get directory name from the data file save location</exception>
    private static void MoveDataFileFromOldPath()
    {
        try
        {
            if (!File.Exists(Constants.OldSavePath) || File.Exists(Constants.SavePath))
            {
                return;
            }

            if (!Directory.Exists(Constants.SavePath))
            {
                Directory.CreateDirectory(
                    Path.GetDirectoryName(Constants.SavePath) ??
                    throw new ArgumentNullException(
                        null,
                        "Couldn't get directory name from the data file save location."));
            }

            File.Move(Constants.OldSavePath, Constants.SavePath);
        }
        catch (Exception)
        {
            // TODO: catch the exception.
        }
    }
}