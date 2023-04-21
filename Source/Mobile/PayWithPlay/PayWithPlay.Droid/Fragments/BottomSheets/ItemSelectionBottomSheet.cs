using Android.Views;
using Google.Android.Material.BottomSheet;
using PayWithPlay.Core.ViewModels;
using PayWithPlay.Droid.Utils;

namespace PayWithPlay.Droid.Fragments.BottomSheets
{
    public abstract class ItemSelectionBottomSheet<TViewModel> : FullBottomSheet<TViewModel> where TViewModel : BaseItemSelectionViewModel
    {
        public override int LayoutId => Resource.Layout.fragment_items_selection;

        public override void OnShow(object? sender, EventArgs e)
        {
            base.OnShow(sender, e);

            var bottomSheet = Dialog!.FindViewById(Resource.Id.design_bottom_sheet)!;
            var bottomSheetBehavior = (DraggableBottomSheetBehavior)BottomSheetBehavior.From(bottomSheet);
            bottomSheetBehavior.PreventDragView = View!.FindViewById<View>(Resource.Id.recycler_containerView);
        }
    }
}
