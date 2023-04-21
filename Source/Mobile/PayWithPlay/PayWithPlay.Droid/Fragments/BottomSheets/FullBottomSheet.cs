using Android.Views;
using Google.Android.Material.BottomSheet;
using MvvmCross.ViewModels;
using PayWithPlay.Droid.Extensions;

namespace PayWithPlay.Droid.Fragments.BottomSheets
{
    public abstract class FullBottomSheet<TViewModel> : BaseBottomSheet<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public override Dialog OnCreateDialog(Bundle? savedInstanceState)
        {
            var dialog = (BottomSheetDialog)base.OnCreateDialog(savedInstanceState);

            dialog.ShowEvent += OnShow;

            return dialog;
        }

        public virtual void OnShow(object? sender, EventArgs e)
        {
            var d = (BottomSheetDialog)sender!;
            d.ShowEvent -= OnShow;

            var bottomSheet = d!.FindViewById(Resource.Id.design_bottom_sheet)!;

            var bottomSheetLayoutParams = bottomSheet.LayoutParameters!;
            bottomSheetLayoutParams.Height = Resources!.DisplayMetrics!.HeightPixels - 40f.ToPx();
            bottomSheet.LayoutParameters = bottomSheetLayoutParams;

            var bottomSheetBehavior = BottomSheetBehavior.From(bottomSheet);
            bottomSheetBehavior.State = BottomSheetBehavior.StateExpanded;
            bottomSheetBehavior.Hideable = true;
            bottomSheetBehavior.SkipCollapsed = true;
        }
    }
}
