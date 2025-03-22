using System.Windows;

namespace MinecraftNotes.Utilities;

public static class ToolBox
{
    public static void PrintException(Exception ex)
    {
        // Print exception if it has occurred.
        
#if DEBUG
        MessageBox.Show(ex.ToString(), $"An exception has occurred ({ex.GetType().FullName})", MessageBoxButton.OK, MessageBoxImage.Error);
#else
        MessageBox.Show(ex.Message, $"An exception has occurred ({ex.GetType().FullName})", MessageBoxButton.OK, MessageBoxImage.Error);
#endif
    }
}