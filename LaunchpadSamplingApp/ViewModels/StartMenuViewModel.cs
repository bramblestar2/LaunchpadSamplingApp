using LaunchpadSamplingApp.Helpers;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace LaunchpadSamplingApp.ViewModels
{
    public class StartMenuViewModel : ViewModelBase
    {
        private ObservableCollection<ProjectFile> _projects = new ObservableCollection<ProjectFile>();
        public ObservableCollection<ProjectFile> Projects
        {
            get { return _projects; }
            set
            {
                _projects = value;
                OnPropertyChanged(nameof(Projects));
            }
        }


        public StartMenuViewModel()
        {
            //ReloadList();
        }


        public void ReloadList()
        {
            ProjectsJsonManager.ReloadProjectFiles();
            Projects = ProjectsJsonManager.ConvertJsonToProjectFile();
        }
    }
}