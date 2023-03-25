using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using MvvmCross.DroidX.RecyclerView;

namespace PayWithPlay.Droid.CustomViews
{
    public class MaxHeightRecyclerView : MvxRecyclerView
    {
        private int _maxHeight = -1;

        #region ctors       

        public MaxHeightRecyclerView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            SetValuesFromAttrs(attrs);
        }

        public MaxHeightRecyclerView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            SetValuesFromAttrs(attrs);
        }

        public MaxHeightRecyclerView(Context context, IAttributeSet attrs, int defStyle, IMvxRecyclerAdapter adapter) : base(context, attrs, defStyle, adapter)
        {
            SetValuesFromAttrs(attrs);
        }

        protected MaxHeightRecyclerView(nint javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        #endregion

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            if (_maxHeight > 0)
            {
                heightMeasureSpec = MeasureSpec.MakeMeasureSpec(_maxHeight, MeasureSpecMode.AtMost);
            }

            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
        }

        private void SetValuesFromAttrs(IAttributeSet attributeSet)
        {
            var attrs = Context!.Theme!.ObtainStyledAttributes(attributeSet, Resource.Styleable.MaxHeightRecyclerView, 0, 0);
            try
            {
                _maxHeight = attrs.GetDimensionPixelSize(Resource.Styleable.MaxHeightRecyclerView_maxHeight, -1);
            }
            finally
            {
                attrs.Recycle();
            }
        }
    }
}
