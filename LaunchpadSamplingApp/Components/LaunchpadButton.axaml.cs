using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Markup.Xaml.Converters;
using Avalonia.Media;
using Avalonia.Styling;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Threading;

namespace LaunchpadSamplingApp.Components
{
    [PseudoClasses(":onPointerEnter", "onPointerLeave", "onPointerPressed", 
                   "onPointerReleased", "audioAdded", "audioRemoved")]
    public partial class LaunchpadButton : UserControl
    {
        private NAudio.Wave.WaveFileReader? _wave = null;
        private NAudio.Wave.DirectSoundOut? _audioOut = null;


        public readonly SolidColorBrush primaryBrush = new SolidColorBrush(Color.FromRgb(0x50, 0x50, 0x50));
        public readonly SolidColorBrush hoveredBrush = new SolidColorBrush(Color.FromRgb(0x40, 0x40, 0x40));
        public readonly SolidColorBrush pressedBrush = new SolidColorBrush(Color.FromRgb(0x20, 0x20, 0x20));


        public LaunchpadButton()
        {
            InitializeComponent();
        }


        public void Play()
        {
            try
            {
                _audioOut?.Play();
            } catch (Exception ex)
            {
                Debug.WriteLine("No Audio in button");
            }
        }


        public void Load(string filepath)
        {
            if (File.Exists(filepath))
            {
                Unload(); //Unload possible audio

                _wave = new NAudio.Wave.WaveFileReader(filepath);
                _audioOut = new NAudio.Wave.DirectSoundOut();
                _audioOut.Init(_wave);
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



        #region Pointer Enter/Exit Events

        protected override void OnPointerEntered(PointerEventArgs e)
        {
            base.OnPointerEntered(e);
            
            PseudoClasses.Set(":onPointerEnter", true);
            PseudoClasses.Set(":onPointerLeave", false);
        }

        protected override void OnPointerExited(PointerEventArgs e)
        {
            base.OnPointerExited(e);

            PseudoClasses.Set(":onPointerLeave", true);
            PseudoClasses.Set(":onPointerEnter", false);
        }

        #endregion

        #region Pointer Press/Release Events

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);

            var pointer = e.GetCurrentPoint(null);

            if (pointer.Properties.IsLeftButtonPressed)
            {
                PseudoClasses.Set(":onPointerPressed", true);
                PseudoClasses.Set(":onPointerReleased", false);
            }
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            base.OnPointerReleased(e);

            var pointer = e.GetCurrentPoint(null);

            if (!pointer.Properties.IsLeftButtonPressed)
            {
                PseudoClasses.Set(":onPointerReleased", true);
                PseudoClasses.Set(":onPointerPressed", false);
            }
        }

        #endregion
    }

}
