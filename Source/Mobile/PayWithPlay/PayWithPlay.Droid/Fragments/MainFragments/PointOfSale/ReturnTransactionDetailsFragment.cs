using Android.Views;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PayWithPlay.Core.ViewModels.Main.PointOfSale.Return;
using PayWithPlay.Droid.Adapters;
using PayWithPlay.Droid.Extensions;
using PayWithPlay.Droid.Utils;

namespace PayWithPlay.Droid.Fragments.MainFragments.PointOfSale
{
    [MvxNavFragmentPresentation(ViewModelType = typeof(ReturnTransactionDetailsViewModel), FragmentMainNavContainerId = Resource.Id.nav_host_container, FragmentNavigationActionId = Resource.Id.action_to_return_transaction_details)]
    public class ReturnTransactionDetailsFragment : BaseFragment<ReturnTransactionDetailsViewModel>
    {
        public override int LayoutId => Resource.Layout.fragment_return_transaction_details;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var root = base.OnCreateView(inflater, container, savedInstanceState);

            var recyclerView = root.FindViewById<MvxRecyclerView>(Resource.Id.rv_items)!;
            recyclerView.AddItemDecoration(new RecyclerItemDecoration(2f.ToPx(), Resource.Color.separator_color));

            recyclerView.Adapter = new TransactionsAdapter((IMvxAndroidBindingContext)BindingContext);

            return root;
        }
    }
}
