using MvvmCross.Platforms.Android.Presenters.Attributes;
using PayWithPlay.Core.ViewModels.Main.Inventory;

namespace PayWithPlay.Droid.Fragments.MainFragments.Inventory
{
    //[MvxFragmentPresentation(ActivityHostViewModelType = typeof(MainViewModel), FragmentContentId = Resource.Id.fragment_container, ViewModelType = typeof(InventoryViewModel))]
    public class InventoryFragment : BaseFragment<InventoryViewModel>
    {
        public override int LayoutId => Resource.Layout.fragment_inventory;
    }
}
