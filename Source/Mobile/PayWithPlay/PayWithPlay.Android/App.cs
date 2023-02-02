using Android.Runtime;
using PayWithPlay.Core;

namespace PayWithPlay.Android
{
    [Application(AllowBackup = false, Theme = "@style/Theme.App.Starting")]
    public class App : Application
    {
        public App(IntPtr handle, JniHandleOwnership ownerShip) : base(handle, ownerShip)
        {
        }

        public static App? Current { get; private set; }

        public override void OnCreate()
        {
            base.OnCreate();
            Current = this;

            ServicesProvider.Current.RegisterDependencies();
        }
    }
}
