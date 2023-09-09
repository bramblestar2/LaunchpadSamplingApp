using Avalonia;
using Avalonia.Threading;
using RtMidi.Core.Devices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchpadSamplingApp.Helpers
{
    static internal class LaunchpadMidiManager
    {
        static private Dictionary<string, IMidiInputDevice> 
            _midiDevices = new Dictionary<string, IMidiInputDevice>();

        static public void ListApis()
        {
            var apis = RtMidi.Core.MidiDeviceManager.Default.GetAvailableMidiApis();

            foreach (var item in apis)
            {
                Debug.WriteLine(item);
            }
        }

        static public void ReloadDeviceList()
        {
            var devices = RtMidi.Core.MidiDeviceManager.Default.InputDevices;

            foreach (var device in devices)
            {
                Debug.WriteLine($"{device.Name}");
            }
        }
    }
}
