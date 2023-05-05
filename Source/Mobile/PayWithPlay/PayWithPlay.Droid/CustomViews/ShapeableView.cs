using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using AndroidX.Core.Content;
using PayWithPlay.Droid.Extensions;

namespace PayWithPlay.Droid.CustomViews
{
    public class ShapeableView : View
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
            var attrs = Context!.Theme!.ObtainStyledAttributes(attributeSet, Resource.Styleable.ShapeableView, 0, 0);

            Color fillColor;
            int cornerRadius;
            try
            {
                fillColor = attrs.GetColor(Resource.Styleable.ShapeableView_fillColor, new Color(ContextCompat.GetColor(Context, Resource.Color.secondary_color)));
                cornerRadius = attrs.GetDimensionPixelSize(Resource.Styleable.ShapeableView_cornerRadius, 0);

            }
            finally
            {
                attrs.Recycle();
            }

            this.SetBackground(ColorStateList.ValueOf(fillColor), cornerRadius: cornerRadius);
        }
    }
}
