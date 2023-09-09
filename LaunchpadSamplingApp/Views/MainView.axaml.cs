using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Input.Raw;
using LaunchpadSamplingApp.Components;
using System;
using System.Diagnostics;
using System.Numerics;

namespace LaunchpadSamplingApp.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }



    private void LaunchpadDrag(object sender, PointerEventArgs e)
    {
        Launchpad launchpad = (Launchpad)sender;

        PointerPoint pointer = e.GetCurrentPoint(this);

        if (launchpad != null && pointer.Properties.IsLeftButtonPressed)
        {
            Canvas.SetLeft(launchpad, pointer.Position.X);
            Canvas.SetTop(launchpad, pointer.Position.Y);
        }
    }
}
