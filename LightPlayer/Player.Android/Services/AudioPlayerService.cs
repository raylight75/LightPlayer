using System;
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
        private static readonly object padlock = new object();
        private static MediaPlayer mediaPlayer = null;                      
        public Action OnFinishedPlaying { get; set; }

        public static MediaPlayer Instance
        {
            get
            {
                lock (padlock)
                {
                    if (mediaPlayer == null)
                    {
                        mediaPlayer = new MediaPlayer();
                    }
                    return mediaPlayer;
                }
            }
        }

        public void Play(string aux)
        {
            if (mediaPlayer == null)
            {
                mediaPlayer = Instance;
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
                mediaPlayer.Completion += MediaPlayer_Completion;
            }
            catch (Exception ex)
            {
                //unable to start playback log error
                Console.WriteLine("Unable to start playback: " + ex);
            }
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
    }
}