using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using LaunchpadSamplingApp.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchpadSamplingApp.ViewModels;

public class LaunchpadViewModel : ViewModelBase
{
    private ObservableCollection<string> _deviceNameList = new ObservableCollection<string>();

    public ObservableCollection<string> DeviceNameList
    {
        get => _deviceNameList;
        private set
        {
            _deviceNameList = value;
        }
    }

    public void UpdateLaunchpadList()
    {
        DeviceNameList.Clear();
        foreach (var device in LaunchpadMidiManager.MidiDevices)
        {
            DeviceNameList.Add(device.Name);
        }
    }
}
