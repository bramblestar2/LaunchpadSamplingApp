using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using NAudio.Wave;
using System;
using System.Diagnostics;
using System.Numerics;

namespace LaunchpadSamplingApp.Components
{
    public partial class Launchpad : UserControl
    {
        public Launchpad()
        {
            InitializeComponent();

            UpdateLaunchpadLayout("Pro");
        }


        private void UpdateLaunchpadLayout(string type)
        {
            switch (type)
            {
                case "Pro":
                    LaunchpadProLayout();
                    break;
            }
        }

        private void LaunchpadProLayout()
        {
            for (int columns = 0; columns < 10; columns++)
                PART_Grid.ColumnDefinitions.Add(new ColumnDefinition());
            for (int rows = 0; rows < 10; rows++)
                PART_Grid.RowDefinitions.Add(new RowDefinition());

            for (int x = 0; x < 10; x++)
                for (int y = 0; y < 10; y++)
                {
                    //Don't create corner buttons
                    if (!(x == 0 && y == 0) && 
                        !(x == 9 && y == 0) &&
                        !(x == 0 && y == 9) &&
                        !(x == 9 && y == 9))
                    {
                        LaunchpadButton button = new LaunchpadButton();
                        button.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch;
                        button.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch;
                        button.Margin = new Thickness(2);
                        button.Width = 20;
                        button.Height = 20;

                        //Create circular buttons on outer
                        if (x == 0 || x == 9 ||
                            y == 0 || y == 9)
                        {
                            button.PART_Border.CornerRadius = new CornerRadius(200);
                            button.Margin = new Thickness(3);
                        }

                        Grid.SetRow(button, x);
                        Grid.SetColumn(button, y);

                        PART_Grid.Children.Add(button);

                        button.PointerPressed += (sender, e) =>
                        {
                            LaunchpadButton lb = sender as LaunchpadButton;

                            lb.Play();
                            //Vector2 position = new Vector2(Grid.GetColumn(lb), Grid.GetRow(lb));
                            //Debug.WriteLine(position);
                        };
                    }
                }
        }


        private void LaunchpadRotateSlider(object? sender, Avalonia.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            TransformGroup launchpadGroup = new TransformGroup();
            launchpadGroup.Children.Add(new RotateTransform(-e.NewValue));

            layoutTransform.LayoutTransform = launchpadGroup;


            TransformGroup logoGroup = new TransformGroup();
            logoGroup.Children.Add(new RotateTransform(e.NewValue));

            NovationLogo.RenderTransform = logoGroup;
        }
    }
}
