using Android.Runtime;
using MvvmCross.Platforms.Android.Views;
using PayWithPlay.Core;

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
        }
    }
}
