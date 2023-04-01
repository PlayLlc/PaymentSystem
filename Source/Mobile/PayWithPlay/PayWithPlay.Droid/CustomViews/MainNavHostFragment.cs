using Android.Runtime;
using AndroidX.Navigation.Fragment;

namespace PayWithPlay.Droid.CustomViews
{
    [Register("MainNavHostFragment")]
    public class MainNavHostFragment : NavHostFragment
    {
        public MainNavHostFragment()
        {
        }

        public MainNavHostFragment(nint javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }
    }
}
