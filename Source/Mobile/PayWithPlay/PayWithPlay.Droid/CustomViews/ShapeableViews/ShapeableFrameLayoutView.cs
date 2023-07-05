using Android.Content;
using Android.Runtime;
using Android.Util;
using PayWithPlay.Droid.Extensions;

namespace PayWithPlay.Droid.CustomViews.ShapeableViews
{
    internal class ShapeableFrameLayoutView : FrameLayout
    {
        #region ctors

        public ShapeableFrameLayoutView(Context? context) : base(context)
        {
        }

        public ShapeableFrameLayoutView(Context? context, IAttributeSet? attrs) : base(context, attrs)
        {
            Init(attrs);
        }

        public ShapeableFrameLayoutView(Context? context, IAttributeSet? attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init(attrs);
        }

        public ShapeableFrameLayoutView(Context? context, IAttributeSet? attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Init(attrs);
        }

        protected ShapeableFrameLayoutView(nint javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        #endregion

        private void Init(IAttributeSet attributeSet)
        {
            this.InitShapeableView(attributeSet);
        }
    }
}
