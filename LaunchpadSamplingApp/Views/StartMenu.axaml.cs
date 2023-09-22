using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Avalonia.VisualTree;
using LaunchpadSamplingApp.Components;
using LaunchpadSamplingApp.CustomArgs;
using LaunchpadSamplingApp.Helpers;
using LaunchpadSamplingApp.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LaunchpadSamplingApp.Views
{
    public partial class StartMenu : UserControl
    {
        private StartMenuViewModel? viewModel = null;

        public StartMenu()
        {
            InitializeComponent();

            viewModel = DataContext as StartMenuViewModel;
        }




        public static readonly RoutedEvent<RoutedEventArgs> NewClickEvent =
            RoutedEvent.Register<StartMenu, RoutedEventArgs>(nameof(NewClick), RoutingStrategies.Bubble);
        public static readonly RoutedEvent<OpenProjectEventArgs> OpenClickEvent =
            RoutedEvent.Register<StartMenu, OpenProjectEventArgs>(nameof(OpenClick), RoutingStrategies.Bubble);

        public event EventHandler<OpenProjectEventArgs> OpenClick
        {
            add => AddHandler(OpenClickEvent, value);
            remove => RemoveHandler(OpenClickEvent, value);
        }

        public event EventHandler<RoutedEventArgs> NewClick
        {
            add => AddHandler(NewClickEvent, value);
            remove => RemoveHandler(NewClickEvent, value);
        }


        private async void onButtonClick(object? sender, RoutedEventArgs e)
        {
            Button? button = (Button?)sender;

            if (button != null)
            {
                string? name = button.Name;

                if (name != null)
                {
                    RoutedEventArgs args;

                    switch (name.ToLower())
                    {
                        case "newbutton":
                            args = new RoutedEventArgs(NewClickEvent);
                            RaiseEvent(args);
                            break;
                        case "openbutton":

                            var topLevel = TopLevel.GetTopLevel(this);

                            var type = new FilePickerFileType("Project File")
                            {
                                Patterns = new[] { "*.disableton", },
                            };

                            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                            {
                                Title = "Open File",
                                AllowMultiple = false,
                                FileTypeFilter = new[] { type },
                            });

                            if (files.Count > 0)
                            {
                                string fullpath = files[0].TryGetLocalPath();
                                string folderpath = Path.GetDirectoryName(fullpath);
                                string filename = Path.GetFileName(fullpath);
                                string imageext = string.Empty;

                                foreach (var item in Directory.EnumerateFiles(folderpath))
                                {
                                    if (Path.GetFileNameWithoutExtension(item) == "icon")
                                    {
                                        imageext = Path.GetExtension(item);
                                        break;
                                    }
                                    Debug.WriteLine(item);
                                };

                                OpenProjectEventArgs openArg = new OpenProjectEventArgs(OpenClickEvent, folderpath, filename, ProjectStatus.UNASSIGNED);
                                RaiseEvent(openArg);
                            }

                            break;
                    }
                }
            }
        }

        protected override void OnLoaded(RoutedEventArgs e)
        {
            base.OnLoaded(e);

            viewModel?.ReloadList();
        }


        private void RemoveProjectFromList(object? sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                int index = ProjectList.SelectedIndex;

                ProjectsJsonManager.RemoveProjectInList(index);
                if (this.DataContext is not null && this.DataContext is StartMenuViewModel)
                {
                    viewModel.Projects.RemoveAt(index);
                }
            }
        }


        private void SaveProjectState(object? sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                bool updateList = false;

                int index = ProjectList.SelectedIndex;
                var item = ProjectsJsonManager.JsonProjectList[index];
                string? imagePath = null;

                if (button.Parent is not null && button.Parent is StackPanel panel)
                {
                    
                    if (panel.Children[0] is Grid grid)
                    {
                        if (grid.Children[1] is ComboBox status)
                        {
                            if (status.SelectedItem is ComboBoxItem statusItem)
                            {

                                if (statusItem.Content is string textStatus)
                                    item.Status = (ProjectStatus)ProjectFile.StringToProjectStatus(textStatus);

                                updateList = true;
                            }
                        }
                    }


                    if (panel.Children[2] is Grid grid2)
                    {
                        if (grid2.Children[0] is ImageGetter imageGetter)
                        {
                            if (imageGetter.ImagePath is string image)
                            {
                                imagePath = image;

                                updateList = true;
                            }
                        }
                    }
                }

                if (updateList)
                {
                    ProjectsJsonManager.SetProjectFile(index, item);

                    if (imagePath is not null)
                    {
                        foreach (var file in Directory.EnumerateFiles(viewModel.Projects[index].Path))
                        {
                            if (Path.GetFileNameWithoutExtension(file) == "icon")
                                File.Delete(file);
                        }
                        string newFileName = viewModel.Projects[index].Path + "\\icon" +
                                             Path.GetExtension(imagePath);
                        File.Copy(imagePath, newFileName, true);
                    }

                    if (this.DataContext is not null && this.DataContext is StartMenuViewModel)
                    {
                        viewModel.Projects[index] = ProjectsJsonManager.JsonProjectList[index];

                        var project = viewModel.Projects.FirstOrDefault(i => i.Name == item.Name);

                        ProjectList.SelectedIndex = index;
                    }
                }
            }
        }
    }
}