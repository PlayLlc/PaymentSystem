﻿using Android.Content.PM;
using PayWithPlay.Core.ViewModels.Welcome;

namespace PayWithPlay.Android.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.App.Starting", MainLauncher = true, ScreenOrientation = ScreenOrientation.UserPortrait)]
    public class WelcomeActivity : BaseActivity<WelcomeViewModel>
    {
        protected override int LayoutId => Resource.Layout.activity_welcome;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            AndroidX.Core.SplashScreen.SplashScreen.InstallSplashScreen(this);

            base.OnCreate(savedInstanceState);
        }
    }
}