using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using LaunchpadSamplingApp.CustomArgs;
using LaunchpadSamplingApp.Helpers;
using LaunchpadSamplingApp.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

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

                                OpenProjectEventArgs openArg = new OpenProjectEventArgs(OpenClickEvent, folderpath, filename);
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


        private void ItemStatusFlyoutClosed(object? sender, EventArgs e)
        {
        }


        private void ItemStatusSelectionChanged(object? sender, SelectionChangedEventArgs e) 
        {
            int index = ProjectList.SelectedIndex;
            var item = ProjectsJsonManager.JsonProjectList[index];
            var combobox = sender as ComboBox;
                
            if (combobox is not null)
            {
                var statusItem = combobox.Items[combobox.SelectedIndex] as ComboBoxItem;
                
                item.Status = (ProjectStatus)ProjectFile.StringToProjectStatus((string)statusItem.Content);

                ProjectsJsonManager.SetProjectFile(index, item);

                if (this.DataContext is not null && this.DataContext is StartMenuViewModel)
                {
                    viewModel.Projects[index] = ProjectsJsonManager.JsonProjectList[index];

                    var project = viewModel.Projects.FirstOrDefault(i => i.Name == item.Name);
                    //
                    //project.Status = item.Status;

                    try
                    {
                        if (project.ImagePath != string.Empty &&
                            project.Image == null)
                        {
                            project.Image = new Bitmap(project.ImagePath);
                            Debug.WriteLine("A");
                        }
                        viewModel.Projects[index] = project;
                    }
                    catch
                    {
                        Debug.WriteLine("Error making Bitmap");
                    }

                    ProjectList.SelectedIndex = index;
                }
            }
        }
    }
}
