using MvvmCross.Platforms.Android.Presenters.Attributes;
using PayWithPlay.Core.ViewModels.Main.Home;

namespace PayWithPlay.Droid.Fragments.MainFragments.Home
{
    //[MvxFragmentPresentation(ActivityHostViewModelType = typeof(MainViewModel), FragmentContentId = Resource.Id.fragment_container, ViewModelType = typeof(HomeViewModel))]
    public class HomeFragment : BaseFragment<HomeViewModel>
    {
        public override int LayoutId => Resource.Layout.fragment_home;
    }
}
