using Android.Views;
using MvvmCross.DroidX.RecyclerView;
using PayWithPlay.Core.ViewModels;
using PayWithPlay.Droid.Extensions;
using PayWithPlay.Droid.Utils;

namespace PayWithPlay.Droid.Fragments.BottomSheets
{
    public class OptionsBottomSheet<TViewModel> : BaseBottomSheet<TViewModel> where TViewModel : OptionsBottomSheetViewModel
    {
        public override int LayoutId => Resource.Layout.fragment_bottom_sheet_options;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var root = base.OnCreateView(inflater, container, savedInstanceState);

            var recyclerView = root.FindViewById<MvxRecyclerView>(Resource.Id.rv_options)!;
            recyclerView.AddItemDecoration(new RecyclerItemDecoration(2f.ToPx(), Resource.Color.separator_color, 24f.ToPx(), 24f.ToPx()));

            return root;
        }
    }
}