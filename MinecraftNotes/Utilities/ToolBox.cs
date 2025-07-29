using System.Windows;

namespace MinecraftNotes.Utilities;

/// <summary>
/// Used for common functions that are repeated and used many times throughout the application.
/// </summary>
public static class ToolBox
{
    /// <summary>
    /// Print exception if it has occurred.
    /// </summary>
    /// <param name="ex">An object containing the exception stack trace.</param>
    public static void PrintException(Exception ex)
    {
        string title = $"An exception has occurred ({ex.GetType().FullName})";
        
#if DEBUG
        MessageBox.Show(ex.ToString(), title, MessageBoxButton.OK, MessageBoxImage.Error);
#else
        MessageBox.Show(ex.Message, title, MessageBoxButton.OK, MessageBoxImage.Error);
#endif
    }
}