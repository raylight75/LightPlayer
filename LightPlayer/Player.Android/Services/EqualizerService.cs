using System;
using System.Collections.Generic;
using Android.Media;
using Android.Media.Audiofx;
using Player.Droid.Services;
using Player.Interfaces;
using Player.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(EqualizerService))]
namespace Player.Droid.Services
{
    class EqualizerService : IEqualizerSevice
    {
        private MediaPlayer mediaPlayer;
        private Equalizer equalizer;

        public EqualizerService()
        {
            mediaPlayer = AudioPlayerService.Instance;
            equalizer = new Equalizer(0, mediaPlayer.AudioSessionId);
            equalizer.SetEnabled(true);
        }

        public List<Bands> SetEqualizer(int preset)
        {
            var result = new List<Bands>();
            if (mediaPlayer == null)
            {
                return result;
            }
            else
            {
                equalizer.UsePreset((short)preset);
                int numberFrequencyBands = equalizer.NumberOfBands;
                //string lowerEqualizerBandLevel = equalizer.GetBandLevelRange()[0] / 100 + "dB";
                short lowerEqualizer = equalizer.GetBandLevelRange()[0];
                short upperEqualizerBandLevel = equalizer.GetBandLevelRange()[1];
                //string upperEqualizerBandLevel = equalizer.GetBandLevelRange()[1] / 100 + "dB";
                int maxValue = (upperEqualizerBandLevel - lowerEqualizer);
                for (short i = 0; i < numberFrequencyBands; i++)
                {
                    short equalizerBandIndex = i;
                    string setFrequency = equalizer.GetCenterFreq(equalizerBandIndex) / 1000 + "Hz";   //// 60-14000Hz
                    Console.WriteLine(equalizerBandIndex);
                    int value = equalizer.GetBandLevel(equalizerBandIndex) - lowerEqualizer; // init value
                    Bands bands = new Bands(setFrequency, equalizerBandIndex, value, maxValue);
                    result.Add(bands);
                }
                return result;
            }
        }

        public void SetBandLevel(int index, int progress)
        {
            if (equalizer == null)
            {
                return;
            }
            else
            {
                short lowerEqualizer = equalizer.GetBandLevelRange()[0];
                int result = progress + lowerEqualizer;
                equalizer.SetBandLevel((short)index, (short)result);
            }
        }

        public List<string> SetBands()
        {
            List<string> equalizerPresetNames = new List<string>();

            for (short i = 0; i < equalizer.NumberOfPresets; i++)
            {
                equalizerPresetNames.Add(equalizer.GetPresetName(i));
            }
            return equalizerPresetNames;
        }
    }
}