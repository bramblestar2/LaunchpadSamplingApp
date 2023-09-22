using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Platform;
using Avalonia.Interactivity;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using LaunchpadSamplingApp.Helpers;
using LaunchpadSamplingApp.ViewModels;
using Microsoft.CodeAnalysis;
using System;
using System.Diagnostics;
using System.IO;

namespace LaunchpadSamplingApp.Views
{
    public partial class NewProjectView : UserControl
    {
        private string? _imagePath = null;

        public NewProjectView()
        {
            InitializeComponent();
        }


        #region Events

        public static readonly RoutedEvent<RoutedEventArgs> CreateClickEvent =
            RoutedEvent.Register<StartMenu, RoutedEventArgs>(nameof(CreateClick), RoutingStrategies.Bubble);


        public event EventHandler<RoutedEventArgs> CreateClick
        {
            add => AddHandler(CreateClickEvent, value);
            remove => RemoveHandler(CreateClickEvent, value);
        }


        public static readonly RoutedEvent<RoutedEventArgs> BackClickEvent =
            RoutedEvent.Register<StartMenu, RoutedEventArgs>(nameof(BackClick), RoutingStrategies.Bubble);

        public event EventHandler<RoutedEventArgs> BackClick
        {
            add => AddHandler(BackClickEvent, value);
            remove => RemoveHandler(BackClickEvent, value);
        }

        #endregion


        #region Methods

        private void CreateProject(object? sender, RoutedEventArgs e)
        {
            bool canCreateProject = false;

            bool nameTextHasString = NameTextBox.Text != string.Empty;
            bool hasValidPath = false;

            string? path = ProjectPathBox.Text;
            string? name = NameTextBox.Text;
            if (path is not null && name is not null)
            {
                if (Directory.Exists(path) && !Directory.Exists($"{path}\\{name}"))
                    hasValidPath = true;
            }

            if (nameTextHasString           && 
                hasValidPath                )
            {
                canCreateProject = true;
            }

            if (canCreateProject)
            {
                Directory.CreateDirectory($"{path}\\{name}");
                using (var file = File.Create($"{path}\\{name}\\{name}.disableton"))
                {
                }

                ProjectFile jsonProject = new ProjectFile()
                {
                    Name = $"{name}.disableton",
                    Path = $"{path}\\{name}",
                };

                if (_imagePath is not null)
                {
                    File.Copy(_imagePath, jsonProject.Path + "\\icon" + Path.GetExtension(_imagePath));
                }

                ProjectsJsonManager.AddProjectFile(jsonProject);

                RoutedEventArgs args = new RoutedEventArgs(CreateClickEvent);
                RaiseEvent(args);
            }
        }


        private async void SelectProjectPath(object? sender, RoutedEventArgs e)
        {
            var topLevel = TopLevel.GetTopLevel(this);

            var folder = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
            {
                Title = "Set Project Path",
                AllowMultiple = false,
            });

            if (folder.Count > 0)
            {
                ProjectPathBox.Text = folder[0].TryGetLocalPath();
            }
        }


        private void BackButtonClick(object? sender, RoutedEventArgs e)
        {
            RoutedEventArgs args = new RoutedEventArgs(BackClickEvent);
            RaiseEvent(args);
        }


        private async void SelectProjectImage(object? sender, RoutedEventArgs e)
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
                try
                {
                    _imagePath = path;
                    (this.DataContext as NewProjectViewModel).ProjectImage = new Avalonia.Media.Imaging.Bitmap(path);
                } catch
                {
                    Debug.WriteLine("No File");
                }
            }
        }

        #endregion
    }
}
