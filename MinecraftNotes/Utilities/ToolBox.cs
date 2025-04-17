using System.Windows;

namespace MinecraftNotes.Utilities;

public static class ToolBox
{
    public static void PrintException(Exception ex)
    {
        // Print exception if it has occurred.

        string title = $"An exception has occurred ({ex.GetType().FullName})";
        
#if DEBUG
        MessageBox.Show(ex.ToString(), title, MessageBoxButton.OK, MessageBoxImage.Error);
#else
        MessageBox.Show(ex.Message, title, MessageBoxButton.OK, MessageBoxImage.Error);
#endif
    }
}