using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Player.Droid.Services;
using System;

namespace Player.Droid
{

    [Activity(Label = "LightPlayer", Icon = "@drawable/app", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        internal static MainActivity Instance { get; private set; }       
        public event EventHandler<ActivityResultEventArgs> ActivityResult = delegate { };

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Instance = this;            
            LoadApplication(new App());
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            ActivityResult(this, new ActivityResultEventArgs
            {
                RequestCode = requestCode,
                ResultCode = resultCode,
                IntentData = data
            });
        }
    }
}