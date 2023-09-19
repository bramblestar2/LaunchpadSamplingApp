using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchpadSamplingApp.ViewModels
{
    public class NewProjectViewModel : ViewModelBase
    {
        private Bitmap? _projectImage = null;

        public Bitmap? ProjectImage { 
            get { return _projectImage; } 
            set 
            { 
                _projectImage = value;
                OnPropertyChanged(nameof(ProjectImage));
            }
        }

        public NewProjectViewModel() 
        { 

        }
    }
}
