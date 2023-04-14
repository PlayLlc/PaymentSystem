using Android.Views;
using MvvmCross.DroidX.Material;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PayWithPlay.Core.ViewModels;
using PayWithPlay.Droid.Extensions;
using PayWithPlay.Droid.Utils;

namespace PayWithPlay.Droid.Fragments
{
    public class BottomSheetItemsFragment<TViewModel> : MvxBottomSheetDialogFragment<TViewModel> where TViewModel : BottomSheetItemsViewModel
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _ = base.OnCreateView(inflater, container, savedInstanceState);

            var root = this.BindingInflate(Resource.Layout.fragment_bottom_sheet_items, container, false);

            var recyclerView = root.FindViewById<MvxRecyclerView>(Resource.Id.rv_items)!;
            recyclerView.AddItemDecoration(new RecyclerItemDecoration(2f.ToPx(), Resource.Color.separator_color, 24f.ToPx(), 24f.ToPx()));

            return root;
        }
    }
}
