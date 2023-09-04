using Avalonia.Controls;
using Avalonia.Interactivity;
using Commons.Music.Midi;
using LaunchpadSamplingApp.Helpers.CustomArgs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchpadSamplingApp.Helpers
{
    internal class ManagedMidi : Control
    {
        private IMidiAccess _access = MidiAccessManager.Default;
        private IEnumerable<IMidiPortDetails> _inputPorts;
        private IMidiPortDetails? _currentDeviceDetails;
        private IMidiInput? _currentDeviceInput;

        public IEnumerable<IMidiPortDetails>? InputPorts { get => _inputPorts; }

        public static RoutedEvent MidiInputEvent =
            RoutedEvent.Register<ManagedMidi, MidiInputEventArgs>(nameof(OnMidiInput), RoutingStrategies.Bubble);

        public event EventHandler<MidiInputEventArgs> OnMidiInput
        {
            add { AddHandler(MidiInputEvent, value); }
            remove { RemoveHandler(MidiInputEvent, value); }
        }

        public ManagedMidi()
        {
            this.ReloadDevices();
        }

        public bool SetDevice(string deviceName)
        {
            try
            {
                var found = _inputPorts?.Where(device =>
                {
                    if (device.Id == deviceName)
                        return true;

                    return false;
                }).First();

                if (found != null)
                {
                    this.StopDevice();
                    _currentDeviceDetails = found;
                    _currentDeviceInput = _access.OpenInputAsync(_currentDeviceDetails?.Id).Result;
                    _currentDeviceInput.MessageReceived += (s, e) =>
                    {
                        RaiseEvent(new MidiInputEventArgs(MidiInputEvent, e));
                    };
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("No device found");
            }

            return false;
        }


        public void ReloadDevices()
        {
            _inputPorts = _access.Inputs;
        }


        public void StopDevice()
        {
            if (_currentDeviceInput != null)
            {
                _currentDeviceInput.CloseAsync();
            }
        }
    }
}
