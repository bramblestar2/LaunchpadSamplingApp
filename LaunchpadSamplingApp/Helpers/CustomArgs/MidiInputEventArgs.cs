using Avalonia.Interactivity;
using Commons.Music.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchpadSamplingApp.Helpers.CustomArgs
{
    public enum MidiType
    {
        PRESSED = 159,
        RELEASED = 143,
    }

    internal class MidiInputEventArgs : RoutedEventArgs
    {
        private readonly MidiType _type;
        private readonly int _note;
        private readonly int _velocity;
        public int Note { get { return _note; } }
        public int Velocity { get { return _velocity; } }
        public MidiType Type { get { return _type; } }

        public MidiInputEventArgs(RoutedEvent routedEvent, MidiReceivedEventArgs e) : base(routedEvent)
        {
            _type = (MidiType)e.Data[0];
            _note = e.Data[1];
            _velocity = e.Data[2];
        }
    }
}
