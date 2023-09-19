using Avalonia.Interactivity;
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

        public OpenProjectEventArgs(RoutedEvent e, string loc, string filename) : base(e)
        { 
            FolderLocation = loc;
            FileName = filename;
        }
    }
}
