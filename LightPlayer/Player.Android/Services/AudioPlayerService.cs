using System;
using Android.Graphics;
using Android.Media;
using Player.Droid.Services;
using Xamarin.Forms;
using Player.Interfaces;

[assembly: Dependency(typeof(AudioPlayerService))]
namespace Player.Droid.Services
{
    class AudioPlayerService : IAudioPlayerService
    {
        private MediaPlayer mediaPlayer;
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
                mediaPlayer.Completion += MediaPlayer_Completion;
            }
            catch (Exception ex)
            {
                //unable to start playback log error
                Console.WriteLine("Unable to start playback: " + ex);
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