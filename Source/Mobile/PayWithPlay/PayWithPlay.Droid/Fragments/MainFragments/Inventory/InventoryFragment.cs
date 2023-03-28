using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.Navigation.Fragment;
using PayWithPlay.Core.ViewModels.Main.Inventory;
using PayWithPlay.Droid.Extensions;

namespace PayWithPlay.Droid.Fragments.MainFragments.Inventory
{
    //[MvxFragmentPresentation(ActivityHostViewModelType = typeof(MainViewModel), FragmentContentId = Resource.Id.fragment_container, ViewModelType = typeof(InventoryViewModel))]
    public class InventoryFragment : BaseFragment<InventoryViewModel>
    {
        public override int LayoutId => Resource.Layout.fragment_inventory;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ViewModel.OnAddAction = OnAdd;
            ViewModel.OnSearchAction = OnSearch;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var rootView = base.OnCreateView(inflater, container, savedInstanceState);

            var actionsView = rootView.FindViewById<LinearLayoutCompat>(Resource.Id.actions_container);
            actionsView.SetBackground(Resource.Color.secondary_color, bottomLeft: 5f.ToFloatPx(), bottomRight: 5f.ToFloatPx());

            return rootView;
        }

        public override void OnDestroy()
        {
            ViewModel.OnSearchAction = null;
            ViewModel.OnAddAction = null;

            base.OnDestroy();
        }

        private void OnSearch()
        {
            FragmentKt.FindNavController(this).Navigate(Resource.Id.action_to_search_inventory);
        }

        private void OnAdd()
        {
            FragmentKt.FindNavController(this).Navigate(Resource.Id.action_to_create_item);
        }
    }
}