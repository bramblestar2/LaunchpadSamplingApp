using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace LaunchpadSamplingApp.Components
{
    public partial class AudioSlicer : UserControl
    {
        private NAudio.Wave.AudioFileReader? _wave = null;
        private NAudio.Wave.DirectSoundOut? _audioOut = null;


        public AudioSlicer()
        {
            InitializeComponent();

            DragDrop.SetAllowDrop(this, true);
            AddHandler(DragDrop.DropEvent, DropEvent);
        }


        public void Load(string? path)
        {
            if (File.Exists(path))
            {
                switch (System.IO.Path.GetExtension(path))
                {
                    case ".wav":
                    case ".mp3":
                    case ".ogg":
                    case ".aiff":
                        Unload(); //Unload possible audio
                        
                        _wave = new NAudio.Wave.AudioFileReader(path);
                        _audioOut = new NAudio.Wave.DirectSoundOut();
                        _audioOut.Init(_wave);

                        byte[] bytes = new byte[_wave.Length];

                        _wave.ReadAsync(bytes, 0, bytes.Length).Wait();
                        
                        DrawAudioWaveform(bytes);

                        break;
                }
            }
        }


        private void Unload()
        {
            if (_audioOut != null && _wave != null)
            {
                _wave.Dispose();
                _audioOut.Dispose();

                _wave = null;
                _audioOut = null;
            }
        }


        private void DrawAudioWaveform(byte[] bytes)
        {
            PART_Panel.Children.Clear();

            for (int position = 0; position < bytes.Length; position += _wave.WaveFormat.SampleRate/32)
            {
                //Debug.Write($"{bytes[position]}, ");

                Rectangle rectangle = new Rectangle()
                {
                    Width = 3,
                    MinHeight = 2,
                    Height = bytes[position]/2,
                    MaxHeight = 150,
                    Fill = Brushes.White,
                };

                PART_Panel.Children.Add(rectangle);
            }
        }


        private void WaveformScroll(object? sender, PointerWheelEventArgs e)
        {
            double scrollSpeed = 20;

            switch (e.Delta)
            {
                //Scroll Right
                case Vector(0, 1):
                case Vector(1, 0):
                    
                    PART_Scroll.Offset = Vector.Add(PART_Scroll.Offset, new Vector(scrollSpeed, 0));

                    break;

                //Scroll Left
                case Vector(0, -1):
                case Vector(-1, 0):

                    PART_Scroll.Offset = Vector.Add(PART_Scroll.Offset, new Vector(-scrollSpeed, 0));

                    break;
            }
        }


        private void DropEvent(object? sender, DragEventArgs e)
        {
            IDataObject data = e.Data;
            IEnumerable<IStorageItem>? fileList = data.GetFiles();
            
            if (fileList != null )
            {
                if (fileList.Count() > 0)
                {

                    this.Load(fileList.First().TryGetLocalPath());
                }
            }
        }

    }
}
