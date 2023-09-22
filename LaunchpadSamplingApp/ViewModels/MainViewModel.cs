using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using LaunchpadSamplingApp.Helpers;
using LaunchpadSamplingApp.Views;
using System.Diagnostics;

namespace LaunchpadSamplingApp.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public MainViewModel() 
    {
        View = _startMenu;

        SetupStartupMenuCommands();
        SetupProjectViewCommands();
        SetupNewProjectViewCommands();
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
    private NewProjectView _newProjectView = new NewProjectView();

    private ProjectView _projectView = new ProjectView();


    private void SetupStartupMenuCommands()
    {
        _startMenu.NewClick += (s, e) =>
        {
            _newProjectView = new NewProjectView();
            SetupNewProjectViewCommands();
            View = _newProjectView;
        };
        
        _startMenu.OpenClick += (s, e) =>
        {
            //Debug.WriteLine($"{e.FolderLocation} | {e.FileName}");
            
            ProjectFile file = new ProjectFile()
            {
                Name = e.FileName,
                Path = e.FolderLocation,
                Status = e.Status,
            };

            ProjectsJsonManager.AddProjectFile(file);

            View = _projectView;
        };
    }

    private void SetupNewProjectViewCommands()
    {
        _newProjectView.CreateClick += (s, e) =>
        {
            View = _projectView;
        };

        _newProjectView.BackClick += (s, e) =>
        {
            View = _startMenu;
        };
    }

    private void SetupProjectViewCommands()
    {
        _projectView.backButton.Command = new RelayCommand(() =>
        {
            View = _startMenu;
        });
    }
}
