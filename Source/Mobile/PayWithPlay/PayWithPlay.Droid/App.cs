﻿using Android.Runtime;
using Android.Views.InputMethods;
using Android.Views;
using MvvmCross.Platforms.Android.Views;
using PayWithPlay.Core;
using Microsoft.Maui.ApplicationModel;

namespace PayWithPlay.Droid
{
    [Application(AllowBackup = false, Theme = "@style/Theme.App.Starting")]
    public class App : MvxAndroidApplication<Setup, CoreApp>
    {
        public App(IntPtr handle, JniHandleOwnership ownerShip) : base(handle, ownerShip)
        {
        }

        public static App? Current { get; private set; }

        public override void OnCreate()
        {
            base.OnCreate();
            Current = this;

            Platform.Init(this);
        }

        public void ClearFocusAndHideKeyboard()
        {
            if (!Platform.CurrentActivity!.IsDestroyed &&
                !Platform.CurrentActivity.IsFinishing)
            {
                var focusedView = Platform.CurrentActivity.CurrentFocus;

                if (focusedView != null)
                {
                    var inputManager = (InputMethodManager)Platform.CurrentActivity.GetSystemService(InputMethodService)!;
                    inputManager.HideSoftInputFromWindow(focusedView.WindowToken, HideSoftInputFlags.None);
                    focusedView.ClearFocus();
                }
            }
        }
    }
}