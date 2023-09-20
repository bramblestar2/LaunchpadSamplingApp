using Avalonia.Media.Imaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace LaunchpadSamplingApp.Helpers
{
    static public class ProjectsJsonManager
    {
        static private List<ProjectFile> _jsonProjectList = new List<ProjectFile>();
        static private string _projectListFileName = "projectslist";

        static public List<ProjectFile> JsonProjectList
        {
            get { return _jsonProjectList; }
            private set
            {
                _jsonProjectList = value;
            }
        }


        static public void SaveProjectListToFile()
        {
            //ReloadProjectFiles();

            string dir = Directory.GetCurrentDirectory();
            string projectListFile = _projectListFileName + ".json";

            var json = JsonConvert.SerializeObject(JsonProjectList, Formatting.Indented);

            if (json is not null)
            {
                File.WriteAllText(dir + "\\" + projectListFile, json);
            }
        }


        static private void RemoveNonexistentProjects()
        {
            for (int i = 0; i < JsonProjectList.Count; i++)
            {
                var project = JsonProjectList[i];
                if (!File.Exists($"{project.Path}\\{project.Name}"))
                {
                    JsonProjectList.Remove(project);
                    i--;
                    Debug.WriteLine("E");
                }
            }
        }


        static public void AddProjectFile(ProjectFile projectFile)
        {
            foreach (var project in JsonProjectList)
            {
                if (project.Path     + "\\" + project.Name      == 
                    projectFile.Path + "\\" + projectFile.Name)
                {
                    Debug.WriteLine("Project already exists in list");
                    return;
                }
            }

            JsonProjectList.Add(projectFile);
            SaveProjectListToFile();
        }


        static public void ReloadProjectFiles()
        {
            string dir = Directory.GetCurrentDirectory();
            string projectListFile = _projectListFileName + ".json";

            if (!File.Exists(dir + "\\" + projectListFile))
            {
                using (var file = File.Create(dir + "\\" + projectListFile))
                { }
            }

            LoadProjectFiles(dir + "\\" + projectListFile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileList">A json file containing the locations, names, and possible image path</param>
        static public void LoadProjectFiles(string? fileList)
        {
            if (fileList is not null)
            {
                if (File.Exists(fileList))
                {
                    FileStreamOptions options = new FileStreamOptions() 
                    { 
                        Access = FileAccess.Read,
                        Share = FileShare.ReadWrite,
                        Mode = FileMode.Open,
                    };
                    using (StreamReader r = new StreamReader(fileList, options))
                    {
                        string json = r.ReadToEnd();
                        var list = JsonConvert.DeserializeObject<List<ProjectFile>>(json);
                        if (list is not null)
                        {
                            JsonProjectList = list;
                        }
                    }
                }

                RemoveNonexistentProjects();
            }
        }

        static public ObservableCollection<ProjectFile> ConvertJsonToProjectFile()
        {
            ObservableCollection<ProjectFile> list = new ObservableCollection<ProjectFile>();
            List<ProjectFile> toRemoveList = new List<ProjectFile>();

            if (JsonProjectList is not null)
            {
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

                    if (File.Exists(project.Path + "\\" + project.Name))
                        list.Add(file);
                }
            }
            return list;
        }
    }
}


namespace LaunchpadSamplingApp.Helpers
{
    public enum ProjectStatus
    {
        [EnumMember(Value = "Unassigned")]
        UNASSIGNED = 0x00,
        [EnumMember(Value = "Work In Progress")]
        WIP = 0x01,
        [EnumMember(Value = "Abandoned")]
        ABANDONED = 0x02,
        [EnumMember(Value = "Completed")]
        COMPLETED = 0x03,
    }

    

    public struct ProjectFile
    {
        public string Name { get; set; }
        public string Path { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ProjectStatus Status { get; set; }
        [JsonIgnore]
        public Bitmap? Image { get; set; }
        public string ImagePath { get; set; }

        static public ProjectStatus? StringToProjectStatus(string status)
        {
            ProjectStatus? result = null;

            switch (status)
            {
                case "Unassigned":
                    result = ProjectStatus.UNASSIGNED;
                    break;
                case "Work In Progress":
                    result = ProjectStatus.WIP;
                    break;
                case "Abandoned":
                    result = ProjectStatus.ABANDONED;
                    break;
                case "Completed":
                    result = ProjectStatus.COMPLETED;
                    break;
            }

            return result;
        }
    }
}