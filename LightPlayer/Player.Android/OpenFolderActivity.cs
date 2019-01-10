using System;
using Android.App;
using Android.Content;
using Android.OS;
using Player.Droid;
using Player.Droid.Services;
using Player.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(OpenFolderActivity))]
namespace Player.Droid
{
    [Activity(Label = "OpenFolder", NoHistory = true)]
    public class OpenFolderActivity : Activity,IPathService
    {
        Context context = Android.App.Application.Context;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);            
            // Create your application here
        }

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
            var activity = context as OpenFolderActivity;
            //MainActivity activity = MainActivity.Instance;            
            var intent = new Intent(Intent.ActionOpenDocumentTree);
            activity.StartActivityForResult(intent, 1);            
        }        
    }
}