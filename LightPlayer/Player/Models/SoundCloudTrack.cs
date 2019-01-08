using Player.Helpers;
using System;
using System.ComponentModel;

namespace Player.Models
{
    class SoundCloudTrack : BaseTrack, INotifyPropertyChanged
    {              
        public string _duration;
        public string Title { get; set; }
        public string Artwork_url { get; set; }
        public string Tag_list { get; set; }

        public override string Duration
        {
            get { return TimeSpan.FromMilliseconds(Int32.Parse(_duration)).ToString(@"mm\:ss"); }
            set
            {
                if (value == _duration) return;
                _duration = value;
                OnPropertyChanged(nameof(Duration));
            }
        }        

        public SoundCloudTrack(int index, int id, string duration, string title, string artwork, string tag)
        {
            Index = index;
            Id = id;
            Duration = duration;
            Title = title;
            Artwork_url = artwork;
            Tag_list = artwork;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
