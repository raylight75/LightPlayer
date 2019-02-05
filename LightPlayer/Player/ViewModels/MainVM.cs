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
using Player.Pages;

namespace Player.ViewModels
{
    class MainVM : BaseViewModel
    {
        public MainVM()
        {
            InitApp();
        }

        private void InitApp()
        {
            _audioPlayer = DependencyService.Get<IAudioPlayerService>();
            _equalizers = DependencyService.Get<IEqualizerSevice>();
            _media = DependencyService.Get<IMediaService>();
            _navigationService = DependencyService.Get<INavigationService>();
            Navigation = _navigationService.Navigation;
            playingPage = new Playing();
            songsPage = new Songs();
            _seekerUpdatesPlayer = true;
            AlbumArt = ImageSource.FromFile(FileImages.NoAlbum);
            BandSelected = "Normal";
            SliderMax = 100;
            Equalizers = new BandList();
            _audioPlayer.OnFinishedPlaying = () => { int i = 1; NextSong(i); };
            LoadCommands();
        }

        private void LoadCommands()
        {
            OpenFolderCommand = new RelayCommand(async parameter => { await OpenFolder(); }, Permision.CanExecute);
            OpenDirectoryCommand = new RelayCommand(async parameter => { await OpenDirectory(); }, Permision.CanExecute);
            SearchCommand = new RelayCommand(FilterSongs, Permision.CanExecute);
            SortByCommand = new RelayCommand(SortTrack, Permision.CanExecute);
            FilterGenreCommand = new RelayCommand(async parameter => { await FilterGenre(); }, Permision.CanExecute);
            PlayingSelectedCommand = new RelayCommand(async parameter => { await PlayingSelected(); }, Permision.CanExecute);
            AlbumSelectedCommand = new RelayCommand(async parameter => { await AlbumSelected(); }, Permision.CanExecute);
            ItemSelectedCommand = new RelayCommand(ItemSelected, Permision.CanExecute);
            StreamSelectedCommand = new RelayCommand(StreamSelected, Permision.CanExecute);
            SoundcloudToPlaylistCommand = new RelayCommand(async parameter => { await SoundcloudToPlaylist(); }, Permision.CanExecute);
            PlayCommand = new RelayCommand(Play, Permision.CanExecute);
            ChangeCommand = new RelayCommand(NextSong, Permision.CanExecute);
            ValueChangedCommand = new RelayCommand(ValueChanged, Permision.CanExecute);
            EqualizerChangedCommand = new RelayCommand(EqualizerChanged, Permision.CanExecute);
            BandChangedCommand = new RelayCommand(async parameter => { await BandChanged(); }, Permision.CanExecute);
            ExitAppCommand = new RelayCommand(async parameter => { await ExitApp(); }, Permision.CanExecute);
        }

        private async Task OpenFolder()
        {
            if (Search == null)
            {
                string path = TrackService.OpenPath();
                Song = await TrackService.GetSongs(path);
                if (Song.Count == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("No files in music folder", "Use browse folder button", "ok");
                    return;
                }
                await SetContent();
            }
            else if (Search != null)
            {
                await Application.Current.MainPage.DisplayAlert("Alert", "Songs allready loaded", "ok");
            }
        }

        private async Task OpenDirectory()
        {
            string path = await TrackService.GetPath();
            if (path == "CANCELED")
            {
                await Application.Current.MainPage.DisplayAlert("Caution", "Select folder with music files", "ok");
                return;
            }
            Song = await TrackService.GetSongs(path);
            if (Song.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Alert", "No files in selected folder", "ok");
                return;
            }
            else if (Search != null)
            {
                Song.Clear();
                Song = await TrackService.GetSongs(path);
            }
            await SetContent();
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

        private async Task PlayingSelected()
        {
            playingPage.BindingContext = this;
            await Navigation.PushModalAsync(playingPage);
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

        private void StreamSelected(object p)
        {
            playbackSource = PlaybackSource.Stream;
            var uri = SetUri(SelectedStream.Id);
            PlaySource(uri, SelectedStream.Title, playbackSource);
        }

        private void PlaySource(string path, string name, PlaybackSource playbackSource)
        {
            _audioPlayer.Play(path);
            if (playbackSource == PlaybackSource.Path)
            {
                _media.GetMetadata(path);
                string artist = _media.Artist;
                Label = (string.IsNullOrEmpty(artist)) ? "<Unknow Artist>" : TrackService.SetNames(artist);
                Name = TrackService.SetNames(name);
                TotalTime = SelectedTrack.Duration;
                Album = _media.Album;
                AlbumArt = TrackService.SetImage(path, _audioPlayer);
                Equalizers = _equalizers.SetEqualizer(0);
                Bands = _equalizers.SetBands();
            }
            else if (playbackSource == PlaybackSource.Stream)
            {               
                Label = TrackService.SetNames(SelectedStream.Tag_list);
                Name = TrackService.SetNames(name);
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
                var song = TrackService.GetSongById(Search, SelectedTrack.Id, i);
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

        private void EqualizerChanged(object p)
        {
            int param = Convert.ToInt32(p);
            var result = Equalizers.FirstOrDefault(x => x.BandId == param);
            int value = result.Value;
            _equalizers.SetBandLevel(param, value);
            //Application.Current.MainPage.DisplayAlert("Command", "You have been alerted", "OK");
        }

        private async Task BandChanged()
        {
            if (Bands == null)
            {
                await Application.Current.MainPage.DisplayAlert("Alert", "Play track first", "OK");
            }
            else
            {
                var action = await Application.Current.MainPage.DisplayActionSheet("Select Preset", "Cancel", null, Bands.ToArray());
                int idx = Bands.IndexOf(action.ToString());
                Equalizers = (action == "Cancel") ? _equalizers.SetEqualizer(0) : _equalizers.SetEqualizer(idx);
                BandSelected = (action == "Cancel") ? "Normal" : action.ToString();
            }           
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
                    Search = new ObservableCollection<Track>(TrackService.ReOrder(result));
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
            var result = TrackService.OrderByName(Song,"Name");
            Search = new ObservableCollection<Track>(result);
            var action = await Application.Current.MainPage.DisplayActionSheet("Filter Genres", "Cancel", "Clear Filter", Genre.ToArray());
            var filter = Song.Where(x => x.Genre.ToLower().Contains(action.ToLower())).ToList();
            Search = (action == "Clear Filter" || action == "Cancel") ? Search : new ObservableCollection<Track>(TrackService.ReOrder(filter));
        }

        private void SortTrack(object p)
        {
            if (Search == null)
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
