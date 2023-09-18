using Avalonia.Data.Converters;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LaunchpadSamplingApp.ViewModels
{
    using Avalonia.Controls;
    using StartMenuCustom;

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

        private string _projectListFileName = "projectslist";

        public StartMenuViewModel()
        {
            ReloadProjectFiles();
        }


        public void ReloadProjectFiles()
        {
            string dir = Directory.GetCurrentDirectory();
            string projectListFile = _projectListFileName + ".json";

            if (!File.Exists(dir + "\\" + projectListFile))
            {
                File.Create(dir + "\\" + projectListFile).Close();
            }
            LoadProjectFiles(dir + "\\" + projectListFile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FileList">A json file containing the locations, names, and possible image path</param>
        public async void LoadProjectFiles(string? fileList)
        {
            if (fileList is not null)
            {
                if (File.Exists(fileList))
                {
                    using (StreamReader r = new StreamReader(fileList))
                    {
                        string json = await r.ReadToEndAsync();
                        List<ProjectFileJSON>? files = JsonConvert.DeserializeObject<List<ProjectFileJSON>>(json);

                        if (files is not null)
                        {
                            Projects.Clear();
                            foreach (ProjectFileJSON file in files)
                            {
                                ProjectFile projectFile = new ProjectFile();
                                projectFile.Name = file.Name;
                                projectFile.Path = file.Path;
                                try
                                {
                                    projectFile.Image = new Bitmap(file.ImagePath);
                                } catch
                                {
                                    projectFile.Image = null;
                                }

                                Debug.WriteLine($"{file.Name} | {file.ImagePath} | {file.Path}");
                                Projects.Add(projectFile);
                            }
                        }
                    }
                }
            }
        }

        public void AddProjectToJson(string name, string path, string imagePath)
        {
            
        }
    }
}


namespace LaunchpadSamplingApp.StartMenuCustom
{
    public struct ProjectFile
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public Bitmap? Image { get; set; }
    }

    public struct ProjectFileJSON
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string ImagePath { get; set; }
    }
}