using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform;
using LaunchpadSamplingApp.Helpers;
using System.Diagnostics;

namespace LaunchpadSamplingApp.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        base.OnClosing(e);

        ProjectsJsonManager.SaveProjectListToFile();
    }
}
