using Android.Views;
using Google.Android.Material.BottomSheet;

namespace PayWithPlay.Droid.Utils.Listeners
{
    public class BottomSheetBehaviorListener : BottomSheetBehavior.BottomSheetCallback
    {
        public Action<View, float>? SlideAction { get; set; }
        public Action<View, int>? StateChangedAction { get; set; }

        public override void OnSlide(View bottomSheet, float newState)
        {
            SlideAction?.Invoke(bottomSheet, newState);
        }

        public override void OnStateChanged(View view, int state)
        {
            StateChangedAction?.Invoke(view, state);
        }
    }
}
