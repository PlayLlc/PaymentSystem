using Android.Content;
using Android.Runtime;
using Android.Util;
using PayWithPlay.Droid.Extensions;

namespace PayWithPlay.Droid.CustomViews.ShapeableViews
{
    public class ShapeableView : FrameLayout
    {
        #region ctors

        public ShapeableView(Context? context) : base(context)
        {
        }

        public ShapeableView(Context? context, IAttributeSet? attrs) : base(context, attrs)
        {
            Init(attrs);
        }

        public ShapeableView(Context? context, IAttributeSet? attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init(attrs);
        }

        public ShapeableView(Context? context, IAttributeSet? attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Init(attrs);
        }

        protected ShapeableView(nint javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        #endregion

        private void Init(IAttributeSet attributeSet)
        {
            this.InitShapeableView(attributeSet);
        }
    }
}
