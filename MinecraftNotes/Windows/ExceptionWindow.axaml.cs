using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using MinecraftNotes.Utilities;

namespace MinecraftNotes.Windows;

/// <summary>
/// Window that is displayed when an exception occurs.
/// </summary>
public partial class ExceptionWindow : Window
{
    /// <summary>
    /// Occurred exception data.
    /// </summary>
    private Exception OccurredException { get; }

    /// <summary>
    /// Name of the class where the exception occurred.
    /// </summary>
    private string ClassName { get; }

    /// <summary>
    /// Name of the function where the exception occurred.
    /// </summary>
    private string FunctionName { get; }

    /// <summary>
    /// A constructor of the exception window.
    /// </summary>
    /// <param name="occurredException">Occurred exception data</param>
    /// <param name="className">Name of the class where the exception occurred</param>
    /// <param name="functionName">Name of the function where the exception occurred</param>
    public ExceptionWindow(Exception occurredException, string className, string functionName)
    {
        InitializeComponent();

        OccurredException = occurredException;
        ClassName = className;
        FunctionName = functionName;
    }

    #region Main events

    /// <summary>
    /// Close current window.
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="e">Event arguments</param>
    private async void CloseButton_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            Close();
        }
        catch (Exception exception)
        {
            await ToolBox.PrintException(this, exception, nameof(ExceptionWindow), nameof(CloseButton_OnClick), true);
        }
    }

    #endregion

    #region Window events

    /// <summary>
    /// Handle window's load.
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="e">Event arguments</param>
    private async void Window_OnLoaded(object? sender, RoutedEventArgs e)
    {
        try
        {
            MessageTextBox.Text = OccurredException.Message;
            ClassNameTextBox.Text = ClassName;
            FunctionNameTextBox.Text = FunctionName;
            StackTraceTextBox.Text = OccurredException.ToString();
        }
        catch (Exception exception)
        {
            await ToolBox.PrintException(this, exception, nameof(ExceptionWindow), nameof(Window_OnLoaded), true);
        }
    }

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
            CloseButton_OnClick(sender, e);
        }
        catch (Exception exception)
        {
            await ToolBox.PrintException(this, exception, nameof(ExceptionWindow), nameof(Window_OnKeyDown), true);
        }
    }

    #endregion
}