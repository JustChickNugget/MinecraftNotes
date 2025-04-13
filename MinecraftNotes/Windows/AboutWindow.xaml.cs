using MinecraftNotes.Utilities;
using MinecraftNotes.Other;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Navigation;

namespace MinecraftNotes.Windows;

public partial class AboutWindow
{
    public AboutWindow()
    {
        InitializeComponent();
    }
    
    // Main events
    
    private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
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
    
    // Other events
    
    private void Window_OnLoaded(object sender, RoutedEventArgs e)
    {
        try
        {
            Version appVersion = Assembly.GetExecutingAssembly().GetName().Version
                                 ?? throw new InvalidOperationException("Application version is null.");

            VersionLabel.Content = $"v{appVersion.Major}.{appVersion.Minor}.{appVersion.Build}";
            RepositoryHyperlink.NavigateUri = new Uri(Variables.RepositoryLink);
        }
        catch (Exception ex)
        {
            ToolBox.PrintException(ex);
        }
    }
}