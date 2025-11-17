using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Navigation;
using MinecraftNotes.Other;
using MinecraftNotes.Utilities;

namespace MinecraftNotes.Windows;

/// <summary>
/// A window containing information about the application.
/// </summary>
public partial class AboutWindow
{
    public AboutWindow()
    {
        InitializeComponent();
    }

    #region MAIN EVENTS

    /// <summary>
    /// Hyperlink, that opens repository's page in the browser.
    /// </summary>
    private void RepositoryHyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e)
    {
        try
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri)
            {
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            ToolBox.PrintException(ex);
        }
    }

    #endregion

    #region WINDOW EVENTS

    /// <summary>
    /// Gets application's version from the assembly and sets the repository link to the hyperlink.
    /// </summary>
    private void Window_OnLoaded(object sender, RoutedEventArgs e)
    {
        try
        {
            Version appVersion = Assembly.GetExecutingAssembly().GetName().Version
                                 ?? throw new InvalidOperationException("Application version is null.");

            VersionLabel.Content = $"v{appVersion.Major}.{appVersion.Minor}.{appVersion.Build}";
            RepositoryHyperlink.NavigateUri = new Uri(Constants.RepositoryLink);
        }
        catch (Exception ex)
        {
            ToolBox.PrintException(ex);
        }
    }

    #endregion
}