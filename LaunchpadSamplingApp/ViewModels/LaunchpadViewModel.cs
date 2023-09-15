using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using LaunchpadSamplingApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchpadSamplingApp.ViewModels
{
    public class LaunchpadViewModel : ObservableObject
    {
        private List<string> _deviceList = new List<string>();

        public List<string> DeviceList
        {
            get => _deviceList;
            private set
            {
                _deviceList = value;
                OnPropertyChanged(nameof(DeviceList));
            }
        }

        public void UpdateLaunchpadList()
        {
            DeviceList.Clear();
            foreach (var device in LaunchpadMidiManager.MidiDevices)
            {
                DeviceList.Add(device.Name);
            }
        }
    }
}
