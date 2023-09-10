using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using LaunchpadSamplingApp.Helpers;
using System.ComponentModel;
using System.Diagnostics;

namespace LaunchpadSamplingApp.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();

        LaunchpadMidiManager.ListApis();
        LaunchpadMidiManager.ReloadDeviceList();

    }
}
