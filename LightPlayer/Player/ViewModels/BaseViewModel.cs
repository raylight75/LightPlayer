using Player.Helpers;
using Player.Interfaces;
using Player.Models;
using Player.Pages;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Player.ViewModels
{
    class BaseViewModel : INotifyPropertyChanged
    {
        public PlaybackSource playbackSource;        
        public IAudioPlayerService _audioPlayer;
        public IEqualizerSevice _equalizers;
        public IMediaService _media;               
        public ICommand PageTwoCommand { get; set; }
        public ICommand PlayCommand { get; set; }
        public ICommand StopCommand { get; set; }        
        public ICommand ChangeCommand { get; set; }
        public ICommand ValueChangedCommand { get; set; }        
        public ICommand ShowEqualizerCommand { get; set; }
        public ICommand OpenFolderCommand { get; set; }
        public ICommand OpenDirectoryCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand SortByCommand { get; set; }
        public ICommand FilterGenreCommand { get; set; }
        public ICommand AlbumSelectedCommand { get; set; }
        public ICommand PlayingSelectedCommand { get; set; }
        public ICommand ItemSelectedCommand { get; set; }       
        public ICommand StreamSelectedCommand { get; set; }
        public ICommand SoundcloudToPlaylistCommand { get; set; }
        public ICommand ExitAppCommand { get; set; }
       
        private ImageSource _songImage;       
        public Playing playingPage { get; set; }
        public Songs songsPage { get; set; }
        public Equalizers equalizerPage { get; set; }
        public List<string> Genre { get; set; }        
        private Album _selectedAlbum;
        private Track _currentlySelectedTrack;
        private SoundCloudTrack _currentlySelectedStream;
        private ObservableCollection<Track> _songs;
        private ObservableCollection<Track> _search;
        private ObservableCollection<SoundCloudTrack> _soundcloudlist;       
        private List<Album> _albums;      
        public bool _seekerUpdatesPlayer = false;
        private bool _isPlaying;
        private int _sliderMax;
        private int _sliderValue;
        private int _bandValue;
        private string _name;
        private string _label;
        private string _songTime;
        private string _totalTime;
        private string _query;
        private string _album;
        private string _bandSelected;
        public string _filter;

        public int BandValue
        {
            get { return _bandValue; }
            set
            {
                if (Equals(value, _bandValue)) return;
                _bandValue = value;
                OnPropertyChanged(nameof(BandValue));
            }
        }

        public int SliderValue
        {
            get { return _sliderValue; }
            set
            {
                if (Equals(value, _sliderValue)) return;
                _sliderValue = value;
                OnPropertyChanged(nameof(SliderValue));
            }
        }

        public int SliderMax
        {
            get { return _sliderMax; }
            set
            {
                if (Equals(value, _sliderMax)) return;
                _sliderMax = value;
                OnPropertyChanged(nameof(SliderMax));
            }
        }

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

        public string BandSelected
        {
            get { return _bandSelected; }
            set
            {
                if (Equals(value, _bandSelected)) return;
                _bandSelected = value;
                OnPropertyChanged(nameof(BandSelected));
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

        public string Label
        {
            get { return _label; }
            set
            {
                if (Equals(value, _label)) return;
                _label = value;
                OnPropertyChanged(nameof(Label));
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (Equals(value, _name)) return;
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string SongTime
        {
            get { return _songTime; }
            set
            {
                if (Equals(value, _songTime)) return;
                _songTime = value;
                OnPropertyChanged(nameof(SongTime));
            }
        }

        public string TotalTime
        {
            get { return _totalTime; }
            set
            {
                if (Equals(value, _totalTime)) return;
                _totalTime = value;
                OnPropertyChanged(nameof(TotalTime));
            }
        }       

        public string SetUri(int key)
        {
            var uri = string.Format("https://api.soundcloud.com/tracks/{0}/stream?client_id={1}", key,
                           SensitiveInformation.SoundCloudKey);
            return uri;
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

        public bool IsPlaying
        {
            get { return _isPlaying; }
            set
            {
                if (Equals(value, _isPlaying)) return;
                _isPlaying = value;
                OnPropertyChanged(nameof(IsPlaying));
            }
        }

        public ImageSource AlbumArt
        {
            get { return _songImage; }
            set
            {
                if (Equals(value, _songImage)) return;
                _songImage = value;
                OnPropertyChanged(nameof(AlbumArt));
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
