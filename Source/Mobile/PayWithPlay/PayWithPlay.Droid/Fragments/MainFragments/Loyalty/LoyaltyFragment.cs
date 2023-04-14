using Android.Views;
using AndroidX.AppCompat.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PayWithPlay.Core.ViewModels.Main.Loyalty;
using PayWithPlay.Droid.Extensions;

namespace PayWithPlay.Droid.Fragments.MainFragments.Loyalty
{
    //[MvxFragmentPresentation(ActivityHostViewModelType = typeof(MainViewModel), FragmentContentId = Resource.Id.fragment_container, ViewModelType = typeof(LoyaltyViewModel))]
    public class LoyaltyFragment : BaseFragment<LoyaltyViewModel>
    {
        public override int LayoutId => Resource.Layout.fragment_loyalty;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var rootView = base.OnCreateView(inflater, container, savedInstanceState);

            var actionsView = rootView.FindViewById<LinearLayoutCompat>(Resource.Id.actions_container);
            actionsView.SetBackground(Resource.Color.secondary_color, bottomLeft: 5f.ToFloatPx(), bottomRight: 5f.ToFloatPx());

            return rootView;
        }
    }
}
