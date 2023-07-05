using Android.Content;
using Android.Runtime;
using Android.Util;
using AndroidX.AppCompat.Widget;
using PayWithPlay.Droid.Extensions;

namespace PayWithPlay.Droid.CustomViews.ShapeableViews
{
    public class ShapeableLinearLayoutView : LinearLayoutCompat
    {
        #region ctors

        public ShapeableLinearLayoutView(Context context) : base(context)
        {
        }

        public ShapeableLinearLayoutView(Context context, IAttributeSet? attrs) : base(context, attrs)
        {
            Init(attrs);
        }

        public ShapeableLinearLayoutView(Context context, IAttributeSet? attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init(attrs);
        }

        protected ShapeableLinearLayoutView(nint javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        #endregion

        private void Init(IAttributeSet attributeSet)
        {
            this.InitShapeableView(attributeSet);
        }
    }
}
