using Avalonia.Media.Imaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace LaunchpadSamplingApp.Helpers
{
    static public class ProjectsJsonManager
    {
        static private List<ProjectFileJSON>? _jsonProjectList = null;
        static private string _projectListFileName = "projectslist";

        static public List<ProjectFileJSON>? JsonProjectList
        {
            get { return _jsonProjectList; }
            private set
            {
                JsonProjectList = value;
            }
        }

        static public void ReloadProjectFiles()
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
        /// <param name="fileList">A json file containing the locations, names, and possible image path</param>
        static public async void LoadProjectFiles(string? fileList)
        {
            if (fileList is not null)
            {
                if (File.Exists(fileList))
                {
                    using (StreamReader r = new StreamReader(fileList))
                    {
                        string json = await r.ReadToEndAsync();
                        _jsonProjectList = JsonConvert.DeserializeObject<List<ProjectFileJSON>>(json);
                    }
                }
            }
        }

        static public ObservableCollection<ProjectFile> ConvertJsonToProjectFile()
        {
            ObservableCollection<ProjectFile> list = new ObservableCollection<ProjectFile>();

            if (JsonProjectList is not null)
                foreach (var project in JsonProjectList)
                {
                    ProjectFile file = new ProjectFile()
                    {
                        Name = project.Name,
                        Path = project.Path,
                        Status = project.Status,
                    };

                    try
                    {
                        file.Image = new Bitmap(project.ImagePath);
                    }
                    catch
                    {
                        Debug.WriteLine("Error making Bitmap");
                    }

                    if (File.Exists(project.Path))
                        list.Add(file);
                }

            return list;
        }
    }
}


namespace LaunchpadSamplingApp.Helpers
{
    public enum ProjectStatus
    {
        [EnumMember(Value = "Work In Progress")]
        WIP = 0x00,
        [EnumMember(Value = "Abandoned")]
        ABANDONED = 0x01,
        [EnumMember(Value = "Completed")]
        COMPLETED = 0x02,
    }

    public struct ProjectFile
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public ProjectStatus Status { get; set; }
        public Bitmap? Image { get; set; }
    }

    public struct ProjectFileJSON
    {
        public string Name { get; set; }
        public string Path { get; set; }
        [System.Text.Json.Serialization.JsonConverter(typeof(StringEnumConverter))]
        public ProjectStatus Status { get; set; }
        public string ImagePath { get; set; }
    }
}