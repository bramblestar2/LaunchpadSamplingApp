using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using LaunchpadSamplingApp.ViewModels;
using Restless.WaveForm.Calculators;
using Restless.WaveForm.Renderer;
using Restless.WaveForm.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace LaunchpadSamplingApp.Components
{
    public partial class AudioSlicer : UserControl
    {
        AudioSlicerViewModel _audioSlicerVM = new AudioSlicerViewModel();

        public AudioSlicer()
        {
            InitializeComponent();

            DataContext = _audioSlicerVM;

            

            DragDrop.SetAllowDrop(this, true);
            AddHandler(DragDrop.DropEvent, DropEvent);
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
                    _audioSlicerVM.Load(fileList.First().TryGetLocalPath());
                }
            }
        }

        protected override void OnDataContextBeginUpdate()
        {
            base.OnDataContextBeginUpdate();
        }
    }
}
