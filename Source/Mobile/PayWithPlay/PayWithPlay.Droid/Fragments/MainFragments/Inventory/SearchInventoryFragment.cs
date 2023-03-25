using AndroidX.Navigation.Fragment;
using PayWithPlay.Core.ViewModels.Main.Inventory;

namespace PayWithPlay.Droid.Fragments.MainFragments.Inventory
{
    public class SearchInventoryFragment : BaseFragment<SearchInventoryViewModel>
    {
        public override int LayoutId => Resource.Layout.fragment_search_inventory;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ViewModel.OnBackAction = OnBack;
        }

        public override void OnDestroy()
        {
            ViewModel.OnBackAction = null;

            base.OnDestroy();
        }

        private void OnBack()
        {
            FragmentKt.FindNavController(this).NavigateUp();
        }
    }
}
