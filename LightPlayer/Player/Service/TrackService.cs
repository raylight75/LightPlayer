using Android.Graphics;
using Newtonsoft.Json;
using Player.Helpers;
using Player.Interfaces;
using Player.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Player.Service
{
    class TrackService
    {
        private static ObservableCollection<Track> Songs { get; set; }
        public static List<Album> Albums { get; set; }
        public static List<string> Genres { get; set; }
        public static ImageSource AlbumArt { get; set; }

        static TrackService()
        {
            Songs = new ObservableCollection<Track>();
            Albums = new List<Album>();
            Genres = new List<string>();
        }

        public static string OpenPath()
        {
            string path = DependencyService.Get<IPathService>().InternalFolder;
            return path;
        }

        public async static Task<string> GetPath()
        {
            string path = await DependencyService.Get<IPathService>().OpenFolder();
            return path;
        }

        public static ObservableCollection<Track> GetSongs(IAudioPlayerService _audioPlayer, string path)
        {                        
            DirectoryInfo folder = new DirectoryInfo(path);           
            int i = 1;
            foreach (var di in folder.EnumerateFiles("*.mp3*", SearchOption.AllDirectories))
            {
                int id = i++;
                _audioPlayer.GetMetadata(di.FullName);                
                DateTime creation = File.GetCreationTime(di.FullName);
                TimeSpan ts = TimeSpan.FromMilliseconds(Convert.ToInt32(_audioPlayer.Duration));
                ImageSource image = Image(_audioPlayer.GetImage(di.FullName));
                string albums = _audioPlayer.Album;
                string genres = _audioPlayer.Genre;
                albums = (string.IsNullOrEmpty(albums)) ? "<Unknow Album>" : albums;
                genres = (string.IsNullOrEmpty(genres)) ? "<Unknow Genre>" : genres;                
                Track track = new Track(id, di.Name, di.FullName, ts.ToString(@"mm\:ss"), creation, genres, albums, image);
                SetAlbum(albums, image);
                Songs.Add(track);
                SetGenre(genres);
            }           
            return Songs;
        }

        public static async Task<ObservableCollection<SoundCloudTrack>> GetSoundcloud(string query)
        {           
            var soundcloudlist = new ObservableCollection<SoundCloudTrack>();
            using (var web = new WebClient { Proxy = null })
            {
                var res = JsonConvert.DeserializeObject<ObservableCollection<SoundCloudTrack>>
                    (await web.DownloadStringTaskAsync(string.Format("https://api.soundcloud.com/tracks?q={0}&client_id={1}", query, SensitiveInformation.SoundCloudKey)));
                int i = 1;
                foreach (var result in res)
                {
                    result.Index = i++;
                }                
                return soundcloudlist = res;
            }
        }

        private static ImageSource Image(Bitmap image)
        {
            var bmp = image;
            if (bmp == null)
            {
                return AlbumArt = ImageSource.FromFile(FileImages.NoAlbum);
            }
            else
            {
                var imgsrc = ImageSource.FromStream(() =>
                {
                    MemoryStream ms = new MemoryStream();
                    bmp.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
                    ms.Seek(0L, SeekOrigin.Begin);
                    return ms;
                });
                return AlbumArt = imgsrc;
            }
        }

        public static ImageSource SetImage(string path, IAudioPlayerService _audioPlayer)
        {
            var bmp = _audioPlayer.GetImage(path);
            if (bmp == null)
            {
                return AlbumArt = ImageSource.FromFile(FileImages.NoAlbum);
            }
            else
            {
                var imgsrc = ImageSource.FromStream(() =>
                {
                    MemoryStream ms = new MemoryStream();
                    bmp.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
                    ms.Seek(0L, SeekOrigin.Begin);
                    return ms;
                });
                return AlbumArt = imgsrc;
            }
        }

        private static List<Album> SetAlbum(string albums, ImageSource image)
        {
            Album album = new Album(albums, image);            
            if (!Albums.Any(x => x.Title.Contains(albums)))
            {
                Albums.Add(album);
            }
            return Albums;
        }

        private static List<string> SetGenre(string genres)
        {
            if (!Genres.Contains(genres))
            {
                Genres.Add(genres);
            }            
            return Genres;
        }

        public static IEnumerable<T> GetSongById<T>(ObservableCollection<T> tracks, int id, int index) where T: BaseTrack
        {
            IEnumerable<T> result = null;
            int currentSongIndex = id + index;
            if (currentSongIndex > tracks.Count || currentSongIndex == 0)
            {
                currentSongIndex = 1;
            }
            if (typeof(T).Name == "Track")
            {
                result = tracks.Where(x => x.Id == currentSongIndex);

            }
            else if (typeof(T).Name == "SoundCloudTrack")
            {
                result = tracks.Where(x => x.Index == currentSongIndex);
            }
            return result;
        }

        public static List<T> OrderByName<T>(ObservableCollection<T> tracks, string sort) where T : BaseTrack
        {
            List<T> ordered = new List<T>();
            int i = 1;
            if (sort == "Created")
            {
                ordered = tracks.OrderBy(x => x.Created).ToList();
            }
            else if (sort == "Duration")
            {
                ordered = tracks.OrderBy(x => x.Duration).ToList();
            }
            else if (sort == "Name")
            {
                ordered = tracks.OrderBy(x => x.FriendlyName).ToList();
            }                        
            foreach (var item in ordered)
            {
                int id = i++;
                item.Id = id;
            }

            return ordered;
        }
    }
}
