using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using System;
using System.Diagnostics;
using System.IO;

namespace LaunchpadSamplingApp.Components
{
    [PseudoClasses(":onPointerEnter", ":onPointerLeave", ":onPointerPressed", 
                   ":onPointerReleased", ":audioAdded", ":audioRemoved")]
    public partial class LaunchpadButton : UserControl
    {
        private NAudio.Wave.AudioFileReader? _wave = null;
        private NAudio.Wave.DirectSoundOut? _audioOut = null;


        public LaunchpadButton()
        {
            InitializeComponent();
        }


        public void PressUp()
        {
            PseudoClasses.Set(":onPointerPressed", false);
            PseudoClasses.Set(":onPointerReleased", true);
        }

        public void PressDown()
        {
            PseudoClasses.Set(":onPointerPressed", true);
            PseudoClasses.Set(":onPointerReleased", false);
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
