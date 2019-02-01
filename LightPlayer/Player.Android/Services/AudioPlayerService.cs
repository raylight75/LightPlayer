using System;
using Android.Graphics;
using Android.Media;
using Player.Droid.Services;
using Xamarin.Forms;
using Player.Interfaces;
using Player.Models;
using Android.Media.Audiofx;
using System.Collections.Generic;

[assembly: Dependency(typeof(AudioPlayerService))]
namespace Player.Droid.Services
{
    class AudioPlayerService : IAudioPlayerService
    {
        private MediaPlayer mediaPlayer;
        private Equalizer equalizer;
        public string Album { get; set; }
        public string Artist { get; set; }
        public string Genre{ get; set; }
        public string Duration { get; set; }
        public Action OnFinishedPlaying { get; set; }

        public void Initializer()
        {
            mediaPlayer = new MediaPlayer();           
        }

        public void Play(string aux)
        {
            if (mediaPlayer == null)
            {
                Initializer();
            }
            else if (mediaPlayer != null)
            {
                mediaPlayer.Completion -= MediaPlayer_Completion;
                mediaPlayer.Stop();
            }
            try
            {
                mediaPlayer.Reset();
                mediaPlayer.SetDataSource(aux);
                mediaPlayer.Prepare();                
                mediaPlayer.Start();
                equalizer = new Equalizer(0, mediaPlayer.AudioSessionId);
                equalizer.SetEnabled(true);
                SetEqualizer();
                mediaPlayer.Completion += MediaPlayer_Completion;
            }
            catch (Exception ex)
            {
                //unable to start playback log error
                Console.WriteLine("Unable to start playback: " + ex);
            }
        }
        
        public List<Bands> SetEqualizer()
        {
            var result = new List<Bands>();
            if (mediaPlayer == null)
            {
                return result;
            }
            else
            {
                equalizer.UsePreset(5);
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
                    int value = equalizer.GetBandLevel(equalizerBandIndex)- lowerEqualizer; // init value
                    Bands bands = new Bands(setFrequency, value, maxValue);
                    result.Add(bands);                    
                }
                return result;
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

        public void SelectBand()
        {
            //string preset = equalizer.UsePreset(2);
            int numberFrequencyBands = equalizer.NumberOfBands;
            short lowerEqualizerBandLevel = equalizer.GetBandLevelRange()[0];
            for (short i = 0; i < numberFrequencyBands; i++)
            {
                short equalizerBandIndex = i;
                int value = equalizer.GetBandLevel(equalizerBandIndex)- 15;
            }
        }

        public Bitmap GetImage(string filepath)
        {
            Bitmap image;
            MediaMetadataRetriever albumArt = new MediaMetadataRetriever();
            albumArt.SetDataSource(filepath);           
            try
            {
                byte[] art = albumArt.GetEmbeddedPicture();
                image = BitmapFactory.DecodeByteArray(art, 0, art.Length);
            }
            catch (Exception e)
            {
                image = null;
            }
            return image;
        }

        public void GetMetadata(string filepath)
        {
            MediaMetadataRetriever artistInfo = new MediaMetadataRetriever();
            artistInfo.SetDataSource(filepath);
            Album = artistInfo.ExtractMetadata(MetadataKey.Album);
            Artist = artistInfo.ExtractMetadata(MetadataKey.Artist);
            Genre = artistInfo.ExtractMetadata(MetadataKey.Genre);
            Duration = artistInfo.ExtractMetadata(MetadataKey.Duration);
        }

        public void Seek(int value)
        {
            mediaPlayer.SeekTo(value);
        }

        public int SliderMax()
        {
            return mediaPlayer.Duration;
        }

        public int Position()
        {
            return mediaPlayer.CurrentPosition;
        }

        void MediaPlayer_Completion(object sender, EventArgs e)
        {
            OnFinishedPlaying?.Invoke();            
        }

        public void Pause()
        {
            mediaPlayer?.Pause();
        }

        public void Play()
        {
            mediaPlayer?.Start();
        }

        public void Stop()
        {
            mediaPlayer?.Stop();
        }

        public void Reset()
        {
            mediaPlayer?.Reset();
        }
    }
}