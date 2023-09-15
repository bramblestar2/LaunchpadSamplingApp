using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using LaunchpadSamplingApp.Helpers;
using LaunchpadSamplingApp.ViewModels;
using RtMidi.Core;
using RtMidi.Core.Devices;
using RtMidi.Core.Devices.Infos;
using RtMidi.Core.Enums;
using System;
using System.Diagnostics;
using System.Linq;

namespace LaunchpadSamplingApp.Components
{
    public enum LaunchpadLayout
    {
        XYLayout = 0x01,
        DrumLayout = 0x02
    }

    public partial class Launchpad : UserControl
    {
        private IMidiInputDevice? _device = null;
        private LaunchpadButton[,] _buttons = new LaunchpadButton[10,10];

        private LaunchpadViewModel _launchpadViewModel = new LaunchpadViewModel();

        public IMidiInputDevice? Device 
        {
            get => _device;
            set => _device = value;
        }


        public Launchpad()
        {
            InitializeComponent();

            UpdateLaunchpadLayout("Pro");

            this.DataContext = _launchpadViewModel;
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

            for (int y = 0; y < 10; y++)
                for (int x = 0; x < 10; x++)
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

                        _buttons[y,Math.Abs(x-9)] = button;

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

        private void UpdateLaunchpadComboBox(object? sender, EventArgs e)
        {
            LaunchpadMidiManager.ReloadDeviceList();

            _launchpadViewModel.UpdateLaunchpadList();
        }

        private void LaunchpadInputSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            IMidiInputDeviceInfo? deviceInfo = null;

            if (e.AddedItems.Count > 0)
            {
                ComboBoxItem comboBoxItem = e.AddedItems[0] as ComboBoxItem;
                deviceInfo = LaunchpadMidiManager.GetDeviceByName((string)comboBoxItem.Content);
            }

            if (deviceInfo != null)
            {
                ChangeInputDevice(deviceInfo.CreateDevice());
            }
        }

        private void ChangeInputDevice(IMidiInputDevice device)
        {
            if (_device != null)
            {
                if (_device.IsOpen)
                    _device.Dispose();
            }

            _device = device;

            _device.NoteOn += NoteOn;
            _device.NoteOff += NoteOff;

            device.Open();
        }

        private void NoteOn(IMidiInputDevice sender, in RtMidi.Core.Messages.NoteOnMessage msg)
        {
            var note = int.Parse(msg.Key.DisplayName().Split(' ').Last());

            LaunchpadLayout layout = new LaunchpadLayout();

            if (launchpadLayout.SelectedIndex == 0)
                layout = LaunchpadLayout.XYLayout;
            else if (launchpadLayout.SelectedIndex == 1)
                layout = LaunchpadLayout.DrumLayout;

            var pos = GetButtonPositionFromNote(note, layout);

            if (pos != null)
            {
                Dispatcher.UIThread.Invoke(new Action(() =>
                {
                    try
                    {
                        _buttons[(int)pos.Value.X, (int)pos.Value.Y]?.PressDown();
                    }
                    catch
                    {

                    }
                }));
            }
        }

        private void NoteOff(IMidiInputDevice sender, in RtMidi.Core.Messages.NoteOffMessage msg)
        {
            var note = int.Parse(msg.Key.DisplayName().Split(' ').Last());

            LaunchpadLayout layout = new LaunchpadLayout();

            if (launchpadLayout.SelectedIndex == 0)
                layout = LaunchpadLayout.XYLayout;
            else if (launchpadLayout.SelectedIndex == 1)
                layout = LaunchpadLayout.DrumLayout;

            var pos = GetButtonPositionFromNote(note, layout);

            if (pos != null)
            {
                Dispatcher.UIThread.Invoke(new Action(() =>
                {
                    try
                    {
                        _buttons[(int)pos.Value.X, (int)pos.Value.Y].PressUp();
                    }
                    catch
                    {

                    }
                }));
            }
        }


        // I am incredibly sorry for anyone who decides to look at this
        static public Point? GetButtonPositionFromNote(int note, LaunchpadLayout layout)
        {
            
            switch (layout)
            {
                case LaunchpadLayout.DrumLayout:
                    var pos = Converter.DRtoXY(note);
                    int x = pos % 10;
                    int y = pos / 10;

                    return new Point(x, y);

                case LaunchpadLayout.XYLayout:

                    int notex = note % 10;
                    int notey = note / 10;

                    return new Point(notex, notey);
            }

            return null;
        }
    }
}
