using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using AndroidX.CoordinatorLayout.Widget;
using Google.Android.Material.BottomSheet;

namespace PayWithPlay.Droid.Utils
{
    [Register("PayWithPlay.Droid.Utils.DraggableBottomSheetBehavior")]
    public class DraggableBottomSheetBehavior : BottomSheetBehavior
    {
        #region ctors

        public DraggableBottomSheetBehavior()
        {
        }

        public DraggableBottomSheetBehavior(Context context, IAttributeSet? attrs) : base(context, attrs)
        {
        }

        protected DraggableBottomSheetBehavior(nint javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        #endregion

        public View? PreventDragView { get; set; }

        public override bool OnInterceptTouchEvent(CoordinatorLayout parent, Java.Lang.Object child, MotionEvent e)
        {
            if(PreventDragView != null && parent.IsPointInChildBounds(PreventDragView, (int)e.GetX(), (int)e.GetY()))
            {
                return false;
            }

            return base.OnInterceptTouchEvent(parent, child, e);
        }
    }
}
