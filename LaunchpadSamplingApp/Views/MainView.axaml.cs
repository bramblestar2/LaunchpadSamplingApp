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

    private UserControl _view = new StartMenu();
    public UserControl View 
    { 
        get => _view; 
        set
        {
            _view = value;
        }
    }

    [RelayCommand]
    public void ToStartMenu() 
    {
        Presenter.Content = new StartMenu(); 
    }
    [RelayCommand]
    public void ToUserControl() 
    {
        Presenter.Content = new UserControl1();
    }
}
