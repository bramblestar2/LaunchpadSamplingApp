using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Input.Raw;
using LaunchpadSamplingApp.Components;
using LaunchpadSamplingApp.Helpers;
using Serilog.Configuration;
using System;
using System.Diagnostics;
using System.Numerics;

namespace LaunchpadSamplingApp.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();

        LaunchpadMidiManager.ListApis();
    }
}
