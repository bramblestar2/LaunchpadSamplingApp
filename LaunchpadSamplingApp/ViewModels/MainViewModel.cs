using Avalonia.Automation.Provider;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using LaunchpadSamplingApp.Views;
using System.Diagnostics;

namespace LaunchpadSamplingApp.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public MainViewModel() 
    {
        View = _startMenu;

        _startMenu.NewButton.Command = new RelayCommand(() =>
        {
            View = _projectView;
        });


        _projectView.backButton.Command = new RelayCommand(() => 
        {
            View = _startMenu;
        });
    }

    private UserControl? _view;
    public UserControl? View
    {
        get => _view;
        set 
        { 
            _view = value;
            OnPropertyChanged(nameof(View));
        }
    }

    private StartMenu _startMenu = new StartMenu();

    private ProjectView _projectView = new ProjectView();

}
