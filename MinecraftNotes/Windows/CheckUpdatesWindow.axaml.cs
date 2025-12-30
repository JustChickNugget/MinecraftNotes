using System;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using MinecraftNotes.Other;
using MinecraftNotes.Utilities;

namespace MinecraftNotes.Windows;

/// <summary>
/// Update check window.
/// </summary>
public partial class CheckUpdatesWindow : Window
{
    /// <summary>
    /// A constructor of the update checker window.
    /// </summary>
    public CheckUpdatesWindow()
    {
        InitializeComponent();
    }

    #region Main events

    /// <summary>
    /// Check updates.
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="e">Event arguments</param>
    private async void CheckButton_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            bool updatesAvailable = await ToolBox.CheckForUpdatesAsync();

            if (updatesAvailable)
            {
                UpdateStatusLabel.Content = "New version available";
                DownloadButton.IsEnabled = true;
            }
            else
            {
                UpdateStatusLabel.Content = "Latest version";
                DownloadButton.IsEnabled = false;
            }
        }
        catch (Exception exception)
        {
            UpdateStatusLabel.Content = "Error";
            DownloadButton.IsEnabled = false;

            await ToolBox.PrintException(this, exception, nameof(CheckUpdatesWindow), nameof(CheckButton_OnClick));
        }
    }

    /// <summary>
    /// Open repository link.
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="e">Event arguments</param>
    private async void DownloadButton_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = Constants.RepositoryLink,
                UseShellExecute = true
            });
        }
        catch (Exception exception)
        {
            await ToolBox.PrintException(this, exception, nameof(CheckUpdatesWindow), nameof(DownloadButton_OnClick));
        }
    }

    #endregion

    #region Window events

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
            CheckButton_OnClick(sender, e);
        }
        catch (Exception exception)
        {
            await ToolBox.PrintException(this, exception, nameof(CheckUpdatesWindow), nameof(Window_OnKeyDown));
        }
    }

    #endregion
}