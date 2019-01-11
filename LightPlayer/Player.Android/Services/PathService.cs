using Xamarin.Forms;
using Player.Interfaces;
using Player.Droid.Services;
using Android.Content;
using System.Threading.Tasks;
using Android.App;

[assembly: Dependency(typeof(PathService))]
namespace Player.Droid.Services
{
    class PathService : IPathService
    {
        private TaskCompletionSource<string> taskCompletionSource;

        public string InternalFolder
        {
            get
            {
                //return Android.App.Application.Context.FilesDir.AbsolutePath;
                //return Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
                //return Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath;
                return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryMusic).ToString();
            }
        }

        public async Task<string> OpenFolder()
        {
            taskCompletionSource = new TaskCompletionSource<string>();
            var intent = new Intent(Intent.ActionOpenDocumentTree);
            MainActivity activity = MainActivity.Instance;
            activity.StartActivity(intent, OnActivityResult);
            return await taskCompletionSource.Task;
        }

        private void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            string folderPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            string path = null;
            switch (resultCode)
            {
                case Result.Canceled:
                    path = "CANCELED";
                    break;
                case Result.FirstUser:
                    break;
                case Result.Ok:
                    string systemPath = data.Data.Path;
                    string finalPath = systemPath.Split(':')[1];
                    path = folderPath + "/" + finalPath;
                    break;
                default:
                    break;
            }
            taskCompletionSource.SetResult(path);
        }        
    }
}