using MvvmCross.Platforms.Android.Presenters.Attributes;
using PayWithPlay.Core.ViewModels.Main.Loyalty;

namespace PayWithPlay.Droid.Fragments.MainFragments.Loyalty
{
    //[MvxFragmentPresentation(ActivityHostViewModelType = typeof(MainViewModel), FragmentContentId = Resource.Id.fragment_container, ViewModelType = typeof(LoyaltyViewModel))]
    public class LoyaltyFragment : BaseFragment<LoyaltyViewModel>
    {
        public override int LayoutId => Resource.Layout.fragment_loyalty;
    }
}
