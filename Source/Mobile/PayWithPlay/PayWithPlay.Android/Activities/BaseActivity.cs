using Android.OS;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.Core.View;
using PayWithPlay.Android.Utils.Listeners;

namespace PayWithPlay.Android.Activities
{
    public abstract class BaseActivity : AppCompatActivity
    {
        protected abstract int LayoutId { get; }

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(LayoutId);

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
    }
}