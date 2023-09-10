using RtMidi.Core.Devices.Infos;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LaunchpadSamplingApp.Helpers
{
    static internal class LaunchpadMidiManager
    {
        static private IEnumerable<IMidiInputDeviceInfo> 
            _midiDevices = RtMidi.Core.MidiDeviceManager.Default.InputDevices;

        static public IEnumerable<IMidiInputDeviceInfo> MidiDevices
        { 
            get { return _midiDevices; }
        }

        static public IMidiInputDeviceInfo? GetDeviceByName(string name)
        {
            foreach (var midiDevice in _midiDevices)
            {
                if (midiDevice.Name == name) return midiDevice;
            }

            IEnumerable<IMidiInputDeviceInfo>? device = _midiDevices.Where(device =>
            {
                if (device.Name == name) return true;

                return false;
            });

            if (device != null) return device.First();

            return null;
        }

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
            _midiDevices = RtMidi.Core.MidiDeviceManager.Default.InputDevices;
        }
    }
}
