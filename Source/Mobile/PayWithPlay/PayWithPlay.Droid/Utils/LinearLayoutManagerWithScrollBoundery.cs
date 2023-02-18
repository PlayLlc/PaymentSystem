using Android.Content;
using Android.Runtime;
using Android.Util;
using AndroidX.RecyclerView.Widget;

namespace PayWithPlay.Droid.Utils
{
    public class LinearLayoutManagerWithScrollBoundery : LinearLayoutManager
    {
        #region ctors

        protected LinearLayoutManagerWithScrollBoundery(nint javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public LinearLayoutManagerWithScrollBoundery(Context? context) : base(context)
        {
        }

        public LinearLayoutManagerWithScrollBoundery(Context? context, IAttributeSet? attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        public LinearLayoutManagerWithScrollBoundery(Context? context, int orientation, bool reverseLayout) : base(context, orientation, reverseLayout)
        {
        }

        #endregion

        public Action<int>? OnScrollStateChangedAction { get; set; }

        public bool HorizontalScrollEnabled { get; set; } = true;

        public bool VerticalScrollEnabled { get; set; } = true;

        public override bool CanScrollHorizontally()
        {
            return HorizontalScrollEnabled;
        }

        public override bool CanScrollVertically()
        {
            return VerticalScrollEnabled;
        }

        public override void OnScrollStateChanged(int state)
        {
            base.OnScrollStateChanged(state);
            OnScrollStateChangedAction?.Invoke(state);
        }
    }
}
