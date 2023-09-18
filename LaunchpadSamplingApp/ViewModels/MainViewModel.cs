using Avalonia.Automation.Provider;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.Input;
using LaunchpadSamplingApp.StartMenuCustom;
using LaunchpadSamplingApp.Views;
using System.Collections.ObjectModel;
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
        _startMenu.NewButton.Command = new RelayCommand(() =>
        {
            View = _newProjectView;
        });
        
        _startMenu.OpenButton.Command = new RelayCommand(async () =>
        {
            var topLevel = TopLevel.GetTopLevel(View);

            var type = new FilePickerFileType("Project File")
            {
                Patterns = new[] { "*.disableton", },
            };

            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Open File",
                AllowMultiple = false,
                FileTypeFilter = new[] { type },
            });

            if (files.Count > 0)
            {
                foreach (var file in files)
                {
                    Debug.WriteLine(file.TryGetLocalPath());
                }
            }
        });

        _startMenu.ProjectList.SelectionChanged += (s, e) => {
            if (s is ListBox)
            {
                ListBox list = s as ListBox;
                StartMenuViewModel vm = _startMenu.DataContext as StartMenuViewModel;
                ProjectFile file = vm.Projects[list.SelectedIndex];
                Debug.WriteLine($"{file.Name} | {file.Path}");
            }
        };
    }

    private void SetupNewProjectViewCommands()
    {
        _newProjectView.CreateButton.Command = new RelayCommand(() =>
        {
            View = _projectView;
        });
    }

    private void SetupProjectViewCommands()
    {
        _projectView.backButton.Command = new RelayCommand(() =>
        {
            View = _startMenu;
        });
    }
}
