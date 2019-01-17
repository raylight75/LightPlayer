using Player.Helpers;
using Player.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Player.ViewModels
{
    class BaseViewModel : INotifyPropertyChanged
    {
        public PlaybackSource playbackSource;        
        public ICommand PageTwoCommand { get; set; }
        public ICommand PlayCommand { get; set; }
        public ICommand StopCommand { get; set; }        
        public ICommand ChangeCommand { get; set; }
        public ICommand ValueChangedCommand { get; set; }
        public ICommand OpenFolderCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand SortByCommand { get; set; }
        public ICommand FilterGenreCommand { get; set; }
        public ICommand AlbumSelectedCommand { get; set; }
        public ICommand PlayingSelectedCommand { get; set; }
        public ICommand ItemSelectedCommand { get; set; }       
        public ICommand StreamSelectedCommand { get; set; }
        public ICommand SoundcloudToPlaylistCommand { get; set; }
        public ICommand ExitAppCommand { get; set; }

        public List<string> Genre { get; set; }        
        private Album _selectedAlbum;
        private Track _currentlySelectedTrack;
        private SoundCloudTrack _currentlySelectedStream;
        private ObservableCollection<Track> _songs;
        private ObservableCollection<Track> _search;
        private ObservableCollection<SoundCloudTrack> _soundcloudlist;       
        private List<Album> _albums;
        private string _query;
        private string _album;
        public string _filter;        

        public string Album
        {
            get { return _album; }
            set
            {
                if (Equals(value, _album)) return;
                _album = value;
                OnPropertyChanged(nameof(Album));
            }
        }

        public string Query
        {
            get { return _query; }
            set
            {
                if (Equals(value, _query)) return;
                _query = value;
                OnPropertyChanged(nameof(Query));
            }
        }

        public Album SelectedAlbum
        {
            get { return _selectedAlbum; }
            set
            {
                if (Equals(value, _selectedAlbum)) return;
                _selectedAlbum = value;
                OnPropertyChanged(nameof(SelectedAlbum));
            }
        }

        public Track SelectedTrack
        {
            get { return _currentlySelectedTrack; }
            set
            {
                if (Equals(value, _currentlySelectedTrack)) return;
                _currentlySelectedTrack = value;
                OnPropertyChanged(nameof(SelectedTrack));
            }
        }        

        public SoundCloudTrack SelectedStream
        {
            get { return _currentlySelectedStream; }
            set
            {
                if (Equals(value, _currentlySelectedStream)) return;
                _currentlySelectedStream = value;
                OnPropertyChanged(nameof(SelectedStream));
            }
        }

        public ObservableCollection<Track> Song
        {
            get { return _songs; }
            set
            {
                if (Equals(value, _songs)) return;
                _songs = value;
                OnPropertyChanged(nameof(Song));
            }
        }

        public ObservableCollection<Track> Search
        {
            get { return _search; }
            set
            {
                if (Equals(value, _search)) return;
                _search = value;
                OnPropertyChanged(nameof(Search));
            }
        }

        public ObservableCollection<SoundCloudTrack> Soundcloudlist
        {
            get { return _soundcloudlist; }
            set
            {
                if (Equals(value, _soundcloudlist)) return;
                _soundcloudlist = value;
                OnPropertyChanged(nameof(Soundcloudlist));
            }
        }

        public List<Album> Albums
        {
            get { return _albums; }
            set
            {
                if (Equals(value, _albums)) return;
                _albums = value;
                OnPropertyChanged(nameof(Albums));
            }
        }               

        public string Filter
        {
            get { return _filter; }
            set
            {
                _filter = value;
                OnPropertyChanged(nameof(Filter));
            }
        }       

        public string SetUri(int key)
        {
            var uri = string.Format("https://api.soundcloud.com/tracks/{0}/stream?client_id={1}", key,
                           SensitiveInformation.SoundCloudKey);
            return uri;
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
