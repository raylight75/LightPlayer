using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Player.Commands;
using Player.Models;
using Player.Interfaces;
using Xamarin.Forms;
using Player.Helpers;
using Player.Service;
using Plugin.Connectivity;
using System.Text.RegularExpressions;
using Player.Pages;

namespace Player.ViewModels
{
    class MainVM : BaseViewModel
    {
        public INavigation Navigation { get; set; }
        private INavigationService _navigationService;
        private IAudioPlayerService _audioPlayer;       
        public string _name;
        private ImageSource _songImage;
        private string _label;
        private string _songTime;
        private string _totalTime;
        private int _sliderMax;
        private int _sliderValue;
        private bool _seekerUpdatesPlayer = false;
        private bool _isPlaying;        
        public TrackInfo infoPage { get; set; }
        public Songs songsPage { get; set; }

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

        public string Label
        {
            get
            {
                return _label;
            }
            set
            {
                if (_label != value)
                {
                    _label = value;
                    OnPropertyChanged("Label");
                }
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

        public MainVM()
        {
            InitApp();           
        }

        private void InitApp()
        {
            _audioPlayer = DependencyService.Get<IAudioPlayerService>();
            _navigationService = DependencyService.Get<INavigationService>();
            Navigation = _navigationService.Navigation;           
            infoPage = new TrackInfo();
            songsPage = new Songs();
            _seekerUpdatesPlayer = true;
            AlbumArt = ImageSource.FromFile(FileImages.NoAlbum);
            SliderMax = 100;
            _audioPlayer.OnFinishedPlaying = () => { int i = 1; NextSong(i); };            
            LoadCommands();
        }

        private void LoadCommands()
        {
            OpenFolderCommand = new RelayCommand(async parameter => { await OpenFolder(); }, Permision.CanExecute);
            SearchCommand = new RelayCommand(FilterSongs, Permision.CanExecute);
            SortByCommand = new RelayCommand(SortTrack, Permision.CanExecute);
            FilterGenreCommand = new RelayCommand(async parameter => { await FilterGenre(); }, Permision.CanExecute);
            AlbumSelectedCommand = new RelayCommand(async parameter => { await AlbumSelected(); }, Permision.CanExecute);
            ItemSelectedCommand = new RelayCommand(ItemSelected, Permision.CanExecute);
            FullScreenCommand = new RelayCommand(async parameter => { await FullScreen(); }, Permision.CanExecute);
            StreamSelectedCommand = new RelayCommand(StreamSelected, Permision.CanExecute);
            SoundcloudToPlaylistCommand = new RelayCommand(async parameter => { await SoundcloudToPlaylist(); }, Permision.CanExecute);                                   
            PlayCommand = new RelayCommand(Play, Permision.CanExecute);           
            ChangeCommand = new RelayCommand(NextSong, Permision.CanExecute);
            ValueChangedCommand = new RelayCommand(ValueChanged, Permision.CanExecute);
            ExitAppCommand = new RelayCommand(async parameter => { await ExitApp(); }, Permision.CanExecute);
        }       

        private async Task OpenFolder()
        {           
            if (Search == null)
            {
                string path = TrackService.OpenPath();
                Song = TrackService.GetSongs(_audioPlayer, path);
                if (Song.Count == 0)
                {
                    if (await Application.Current.MainPage.DisplayAlert("No files in music folder", "Open another folder", "Yes", "No"))
                    {
                        path = await TrackService.GetPath();
                        if (path == "CANCELED")
                        {
                            await Application.Current.MainPage.DisplayAlert("Caution", "Select folder with music files", "ok");
                            return;
                        }                        
                        Song = TrackService.GetSongs(_audioPlayer, path);
                        if (Song.Count == 0){ return; }
                        await SetContent();
                        return;
                    }
                    return;
                }
                await SetContent();
            }
            else if (Search != null)
            {
                await Application.Current.MainPage.DisplayAlert("Alert", "Songs allready loaded", "ok");
            }
        }

        private async Task SetContent()
        {
            Search = Song;
            Albums = TrackService.Albums;
            Genre = TrackService.Genres;
            if (await Application.Current.MainPage.DisplayAlert("Songs Loaded", "Would you like to open playlist", "Yes", "No"))
            {
                var currentPage = Navigation.NavigationStack.LastOrDefault() as MainPage;
                currentPage.CurrentPage = currentPage.Children[1];
                currentPage.isLoaded = true;
            }
        }


        private async Task SoundcloudToPlaylist()
        {
            var isConnected = CrossConnectivity.Current.IsConnected;
            if (isConnected == false)
            {
                await Application.Current.MainPage.DisplayAlert("Caution", "No Internet connection", "ok");
                return;
            }            
            Soundcloudlist = await TrackService.GetSoundcloud(Query);           
        }

        private async Task AlbumSelected()
        {            
            var filter = Song.Where(x => x.Album.ToLower().Contains(SelectedAlbum.Title.ToLower())).ToList();
            Search = new ObservableCollection<Track>(TrackService.ReOrder(filter));
            songsPage.BindingContext = this;
            await Navigation.PushAsync(songsPage);                                 
        }

        private void ItemSelected(object p)
        {                       
            playbackSource = PlaybackSource.Path;           
            PlaySource(SelectedTrack.Filepath, SelectedTrack.FriendlyName, playbackSource);
            //Application.Current.MainPage.DisplayAlert("Command", "You have been alerted", "OK");                       
        }

        private async Task FullScreen()
        {
            infoPage.BindingContext = this;
            await Navigation.PushModalAsync(infoPage);           
        }

        private void StreamSelected(object p)
        {
            playbackSource = PlaybackSource.Stream;
            var uri = SetUri(SelectedStream.Id);
            PlaySource(uri,SelectedStream.Title, playbackSource);
        }

        private void PlaySource(string path, string name, PlaybackSource playbackSource)
        {            
            _audioPlayer.Play(path);
            if (playbackSource == PlaybackSource.Path)
            {
                _audioPlayer.GetMetadata(path);
                string artist = _audioPlayer.Artist;
                Label = (string.IsNullOrEmpty(artist)) ? "<Unknow Artist>" : Regex.Replace(artist, "(?<=^.{30}).*", "...");                
                Name = Regex.Replace(name, "(?<=^.{30}).*", "...");
                TotalTime = SelectedTrack.Duration;
                Album = _audioPlayer.Album;
                AlbumArt = TrackService.SetImage(path, _audioPlayer);
            }
            else if(playbackSource == PlaybackSource.Stream)
            {
                Label = Regex.Replace(SelectedStream.Tag_list, "(?<=^.{30}).*", "...");
                Name = Regex.Replace(name, "(?<=^.{30}).*", "...");
                TotalTime = SelectedStream.Duration;
                if (string.IsNullOrEmpty(SelectedStream.Artwork_url))
                {
                    AlbumArt = ImageSource.FromFile(FileImages.NoAlbum);
                }
                else
                {
                    AlbumArt = ImageSource.FromUri(new Uri(SelectedStream.Artwork_url));
                }                              
            };
            SliderMax = _audioPlayer.SliderMax();          
            StartTimer();
            IsPlaying = true;
            
        }

        private void StartTimer()
        {
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                TimeSpan runTime = TimeSpan.FromMilliseconds(_audioPlayer.Position());
                Device.BeginInvokeOnMainThread(() =>
                {
                    SongTime = runTime.ToString(@"mm\:ss");
                    SliderValue = _audioPlayer.Position();
                    _seekerUpdatesPlayer = true;
                });
                return true;
            });
        }

        private void NextSong(object p)
        {
            int i = Convert.ToInt32(p);                    
            if (playbackSource == PlaybackSource.Path)
            {
                var song = TrackService.GetSongById(Search,SelectedTrack.Id,i);
                PlaySource(song.First().Filepath, song.First().FriendlyName, playbackSource);
                SelectedTrack = song.First();
            }
            else if (playbackSource == PlaybackSource.Stream)
            {
                var stream = TrackService.GetSongById(Soundcloudlist, SelectedStream.Index, i);
                var uri = SetUri(SelectedStream.Id);
                PlaySource(uri, stream.First().Title, playbackSource);
                SelectedStream = stream.First();
            };
        }

        private void ValueChanged(object p)
        {           
            if (_seekerUpdatesPlayer)
            {
                _seekerUpdatesPlayer = false;
                return;
            }
            _audioPlayer.Seek(SliderValue);
        }

        private void Play(object p)
        {
            if (IsPlaying == true)
            {
                _audioPlayer.Pause();
                IsPlaying = false;
            }
            else
            {
                _audioPlayer.Play();
                IsPlaying = true;
            }           
        }

        private void FilterSongs(object p)
        {                                   
            if (Search != null)
            {               
                if (string.IsNullOrEmpty(Filter))
                {                    
                    Search = Song;                   
                }
                else if (Filter != null)
                {
                    var result = Song.Where(x => x.FriendlyName.ToLower().Contains(Filter.ToLower())).ToList();
                    Search = new ObservableCollection<Track>(result);                    
                }
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Caution", "Load track first to search", "OK");
                return;
            }            
        }

        private async Task FilterGenre()
        {
            var action = await Application.Current.MainPage.DisplayActionSheet("Filter Genres", "Cancel", "Clear Filter", Genre.ToArray());
            var filter = Song.Where(x => x.Genre.ToLower().Contains(action.ToLower())).ToList();
            Search = (action == "Clear Filter" || action == "Cancel") ? Song : new ObservableCollection<Track>(TrackService.ReOrder(filter));            
        }

        private void SortTrack(object p)
        {
            if(Search == null)
            {
                return;
            }
            var result = TrackService.OrderByName(Song, p.ToString());
            Search = new ObservableCollection<Track>(result);
        }

        private async Task ExitApp()
        {
            if (await Application.Current.MainPage.DisplayAlert("Would you like to close player", "You will need to load playlist again", "Yes", "No"))
            {
                var closer = DependencyService.Get<ICloseApplication>();
                closer?.CloseApp();
            }
        }
    }
}
