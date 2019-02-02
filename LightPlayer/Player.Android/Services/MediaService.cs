using System;
using Android.Graphics;
using Android.Media;
using Player.Droid.Services;
using Player.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(MediaService))]
namespace Player.Droid.Services
{
    class MediaService : IMediaService
    {
        public string Album { get; set; }
        public string Artist { get; set; }
        public string Genre { get; set; }
        public string Duration { get; set; }
        public Bitmap Image { get; set; }

        public void GetMetadata(string filepath)
        {            
            MediaMetadataRetriever artistInfo = new MediaMetadataRetriever();
            artistInfo.SetDataSource(filepath);
            Album = artistInfo.ExtractMetadata(MetadataKey.Album);
            Artist = artistInfo.ExtractMetadata(MetadataKey.Artist);
            Genre = artistInfo.ExtractMetadata(MetadataKey.Genre);
            Duration = artistInfo.ExtractMetadata(MetadataKey.Duration);            
            try
            {
                byte[] art = artistInfo.GetEmbeddedPicture();
                Image = BitmapFactory.DecodeByteArray(art, 0, art.Length);
            }
            catch (Exception e)
            {
                Image = null;
            }
        }
    }
}