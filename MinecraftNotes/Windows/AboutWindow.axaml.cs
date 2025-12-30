using System;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MinecraftNotes.Other;
using MinecraftNotes.Utilities;

namespace MinecraftNotes.Windows;

/// <summary>
/// About application window.
/// </summary>
public partial class AboutWindow : Window
{
    /// <summary>
    /// A constructor of the about window.
    /// </summary>
    public AboutWindow()
    {
        InitializeComponent();
    }

    #region Window events

    /// <summary>
    /// Handle window's load.
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="e">Event arguments</param>
    /// <exception cref="InvalidOperationException">Performed an invalid operation with no data returned</exception>
    private async void Window_OnLoaded(object? sender, RoutedEventArgs e)
    {
        try
        {
            Version appVersion = Assembly.GetExecutingAssembly().GetName().Version ??
                                 throw new InvalidOperationException("Application version is null.");

            VersionLabel.Text = $"v{appVersion.Major}.{appVersion.Minor}.{appVersion.Build}";

            DeveloperHyperlinkButton.NavigateUri = new Uri(Constants.DeveloperLink);
            RepositoryHyperlinkButton.NavigateUri = new Uri(Constants.RepositoryLink);
        }
        catch (Exception exception)
        {
            await ToolBox.PrintException(this, exception, nameof(AboutWindow), nameof(Window_OnLoaded));
        }
    }

    #endregion
}