using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using Restless.WaveForm.Renderer;
using Restless.WaveForm.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchpadSamplingApp.ViewModels;


public partial class AudioSlicerViewModel : ObservableObject
{
    private NAudio.Wave.AudioFileReader? _wave = null;
    private NAudio.Wave.DirectSoundOut? _audioOut = null;

    private Bitmap _waveFormImage;


    public Bitmap WaveFormImage
    {
        get => _waveFormImage;
        private set
        {
            _waveFormImage = value;
            OnPropertyChanged(nameof(WaveFormImage));
        }
    }


    public AudioSlicerViewModel() 
    {
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


    private async void DrawWaveform()
    {
        RenderResult renderResult = null;
        try
        {
            RenderSettings settings = new SineSettings()
            {
                Width = 200,
                AutoWidth = true,
                ZoomX = 1,
                ZoomY = 1,
                VolumeBoost = 1,
            };

            IRenderer renderer = null;
            //renderResult = await WaveFormRenderer.CreateAsync(renderer, _wave, , settings);


        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Problem: {ex.Message}");
        }
    }
}
