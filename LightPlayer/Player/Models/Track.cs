using System;
using Xamarin.Forms;

namespace Player.Models
{
    class Track : BaseTrack
    {
        public string Filepath { get; set; }
        public ImageSource Image { get; set; }
        public override string Duration { get; set; }
        public string Genre { get; set; }
        public string Album { get; set; }

        public Track
            (
            int id,
            string friendlyName,
            string filepath,
            string duration,
            DateTime created,
            string genre,
            string album,
            ImageSource image
            )
        {
            Id = id;
            FriendlyName = friendlyName;
            Filepath = filepath;
            Duration = duration;
            Created = created;
            Genre = genre;
            Album = album;
            Image = image;
        }
    }
}
