using Avalonia.Interactivity;
using LaunchpadSamplingApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchpadSamplingApp.CustomArgs
{
    public class OpenProjectEventArgs : RoutedEventArgs
    {
        public readonly string FolderLocation;
        public readonly string FileName;
        public readonly ProjectStatus Status;

        public OpenProjectEventArgs(RoutedEvent e, string loc, string filename, ProjectStatus projectStatus) : base(e)
        { 
            FolderLocation = loc;
            FileName = filename;
            Status = projectStatus;
        }
    }
}
