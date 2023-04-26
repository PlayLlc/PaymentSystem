using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Microsoft.Maui.ApplicationModel;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.ViewModels;
using PayWithPlay.Droid.Extensions;
using PayWithPlay.Droid.Utils;

namespace PayWithPlay.Droid.Activities
{
    [MvxActivityPresentation]
    public abstract class BaseActivity<TViewModel> : MvxActivity<TViewModel> where TViewModel : class, IMvxViewModel
    {
        protected abstract int LayoutId { get; }

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(LayoutId);

            AddBackCallback();
            SetBackAndTitleView();
            // This code is for making the page fullscreen, i.e. going beneath the status bar, but not beneath navigation view
            //if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
            //{
            //    WindowCompat.SetDecorFitsSystemWindows(Window!, false);
            //    var rootView = FindViewById<ViewGroup>(Resource.Id.root_view);
            //    if (rootView != null)
            //    {
            //        ViewCompat.SetOnApplyWindowInsetsListener(rootView, new OnApplyWindowInsetsListener());
            //    }
            //}
            //else
            //{
            //    Window!.ClearFlags(WindowManagerFlags.TranslucentStatus);
            //    Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            //    Window!.DecorView.SystemUiVisibility = (StatusBarVisibility)(SystemUiFlags.LayoutFullscreen | SystemUiFlags.LayoutStable);
            //}
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected new virtual bool OnBackPressed()
        {
            return true;
        }

        private void AddBackCallback()
        {
            var onBackPressedCallback = new BackPressedCallback();
            onBackPressedCallback.OnBackPressedAction = () =>
            {
                if (OnBackPressed())
                {
                    onBackPressedCallback.Enabled = false;
                    OnBackPressedDispatcher.OnBackPressed();
                }
            };
            OnBackPressedDispatcher.AddCallback(this, onBackPressedCallback);
        }

        private void SetBackAndTitleView()
        {
            var backAndTitleView = FindViewById<View>(Resource.Id.back_and_title_view);
            if (backAndTitleView == null)
            {
                return;
            }

            backAndTitleView.SetBackground(Resource.Color.secondary_color, bottomLeft: 5f.ToFloatPx(), bottomRight: 5f.ToFloatPx());
        }
    }
}