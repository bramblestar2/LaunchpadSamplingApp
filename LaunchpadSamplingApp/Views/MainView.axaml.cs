using Avalonia.Controls;
using LaunchpadSamplingApp.Helpers;

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
