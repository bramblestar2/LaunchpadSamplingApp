using Avalonia.Controls;
using Avalonia.Input;
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
        private NAudio.Wave.WaveFileReader? _wave = null;
        private NAudio.Wave.DirectSoundOut? _audioOut = null;


        public AudioSlicer()
        {
            InitializeComponent();

            DragDrop.SetAllowDrop(this, true);
            AddHandler(DragDrop.DropEvent, DropEvent);
        }


        private void DropEvent(object? sender, DragEventArgs e)
        {
            IDataObject data = e.Data;
            IEnumerable<IStorageItem>? fileList = data.GetFiles();
            
            if (fileList != null )
            {
                if (fileList.Count() > 0)
                {
                    Uri path = fileList.First().Path;
                    
                }
            }
        }
    }
}
