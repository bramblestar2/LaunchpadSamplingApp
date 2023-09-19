using Avalonia.Media.Imaging;
using LaunchpadSamplingApp.Helpers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;

namespace LaunchpadSamplingApp.ViewModels
{
    internal class StartMenuViewModel : ViewModelBase
    {
        private ObservableCollection<ProjectFile> _projects = new ObservableCollection<ProjectFile>();
        public ObservableCollection<ProjectFile> Projects
        {
            get { return _projects; }
            private set
            {
                _projects = value;
                OnPropertyChanged(nameof(Projects));
            }
        }


        public StartMenuViewModel()
        {
            ReloadProjectList();

        }


        public void ReloadProjectList()
        {
            ProjectsJsonManager.ReloadProjectFiles();
            Projects = ProjectsJsonManager.ConvertJsonToProjectFile();
        }
    }
}