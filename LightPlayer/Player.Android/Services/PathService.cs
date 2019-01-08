using Xamarin.Forms;
using Player.Interfaces;
using Player.Droid.Services;
using Android.Content;
using Android.Support.V7.App;
using System;

[assembly: Dependency(typeof(PathService))]
namespace Player.Droid.Services
{
    class PathService : AppCompatActivity, IPathService
    {
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

        public void OpenFolder()
        {           
            MainActivity activity = MainActivity.Instance;
            const int RequestEnableBt = 2;
            var intent = new Intent(Intent.ActionOpenDocumentTree);                       
            activity.StartActivityForResult(intent, RequestEnableBt);
            activity.ActivityResult += HandleActivityResult;
        }

        private void HandleActivityResult(object sender, ActivityResultEventArgs e)
        {
            Console.WriteLine(string.Format("Activity result is {0}", e.ResultCode));          
            string path = e.IntentData.Data.Path;
            string finalPath = path.Split(':')[1];
            string realPath = "/storage/emulated/0/" + finalPath;           
            Console.WriteLine(string.Format("Path result is {0}", realPath));            
        }
    }
}