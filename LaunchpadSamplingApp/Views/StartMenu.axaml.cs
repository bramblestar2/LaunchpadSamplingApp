using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml.Converters;
using LaunchpadSamplingApp.ViewModels;
using System;
using System.Diagnostics;

namespace LaunchpadSamplingApp.Views
{
    public partial class StartMenu : UserControl
    {
        public StartMenu()
        {
            InitializeComponent();
        }


        public static readonly RoutedEvent<RoutedEventArgs> NewClickEvent =
            RoutedEvent.Register<StartMenu, RoutedEventArgs>(nameof(NewClick), RoutingStrategies.Bubble);
        public static readonly RoutedEvent<RoutedEventArgs> OpenClickEvent =
            RoutedEvent.Register<StartMenu, RoutedEventArgs>(nameof(NewClick), RoutingStrategies.Bubble);

        public event EventHandler<RoutedEventArgs> NewClick
        {
            add => AddHandler(NewClickEvent, value);
            remove => RemoveHandler(NewClickEvent, value);
        }


        private void onButtonClick(object? sender, RoutedEventArgs e)
        {
            Button? button = (Button?)sender;

            if (button != null)
            {
                string? name = button.Name;

                if (name != null)
                {
                    switch (name.ToLower())
                    {
                        case "newbutton":
                            break;
                        case "openbutton":
                            break;
                    }
                }
            }
        }

        protected override void OnLoaded(RoutedEventArgs e)
        {
            base.OnLoaded(e);

            (this.DataContext as StartMenuViewModel).ReloadProjectFiles();
        }
    }
}
