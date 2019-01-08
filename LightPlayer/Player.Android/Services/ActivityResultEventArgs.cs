using System;
using Android.App;
using Android.Content;

namespace Player.Droid.Services
{
    public class ActivityResultEventArgs : EventArgs
    {
        public int RequestCode { get; set; }
        public Result ResultCode { get; set; }
        public Intent IntentData { get; set; }

        public ActivityResultEventArgs() : base()
        {
        }
    }
}