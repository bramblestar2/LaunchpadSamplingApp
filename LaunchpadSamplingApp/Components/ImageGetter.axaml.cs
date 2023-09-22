using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using LaunchpadSamplingApp.ViewModels;
using System.Diagnostics;

namespace LaunchpadSamplingApp.Components
{
    public partial class ImageGetter : UserControl
    {
        public string? ImagePath
        {
            get
            {
                if (this.DataContext is ImageGetterViewModel vm)
                {
                    return vm.ImagePath;
                }
                else
                    return null;
            }
        }


        public ImageGetter()
        {
            InitializeComponent();
        }


        private async void SetPressed(object? sender, RoutedEventArgs e)
        {
            var topLevel = TopLevel.GetTopLevel(this);

            var type = new FilePickerFileType("Images")
            {
                Patterns = new[] { "*.png", "*.jpg", },
            };

            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Set Project Image",
                AllowMultiple = false,
                FileTypeFilter = new[] { type },
            });

            if (files.Count > 0)
            {
                string path = files[0].TryGetLocalPath();
                (this.DataContext as ImageGetterViewModel).ImagePath = path;
            }
        }


        private void ResetPressed(object? sender, RoutedEventArgs e)
        {
            (this.DataContext as ImageGetterViewModel).ImagePath = null;
        }
    }
}


namespace LaunchpadSamplingApp.ViewModels
{
    public class ImageGetterViewModel : ViewModelBase
    {
        private string? _imagePath = null;
        public string? ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                OnPropertyChanged(nameof(ImagePath));
            }
        }

        public ImageGetterViewModel() 
        {

        }
    }
}