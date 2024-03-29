﻿using Android.Animation;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Com.Airbnb.Lottie;

namespace Player.Droid
{
    [Activity(Label = "LightPlayer", Icon = "@drawable/app", Theme = "@style/splashscreen", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnResume()
        {
            base.OnResume();
            StartActivity(typeof(MainActivity));
        }
    }
    //public class SplashActivity : Activity, Animator.IAnimatorListener
    //{
    //    protected override void OnCreate(Bundle savedInstanceState)
    //    {
    //        base.OnCreate(savedInstanceState);
    //        SetContentView(Resource.Layout.Activity_Splash);

    //        var animationView = FindViewById<LottieAnimationView>(Resource.Id.animation_view);
    //        animationView.AddAnimatorListener(this);
    //    }

    //    public void OnAnimationCancel(Animator animation)
    //    {
    //    }

    //    public void OnAnimationEnd(Animator animation)
    //    {
    //        StartActivity(new Intent(Application.Context, typeof(MainActivity)));
    //    }

    //    public void OnAnimationRepeat(Animator animation)
    //    {
    //    }

    //    public void OnAnimationStart(Animator animation)
    //    {
    //    }
    //}
}