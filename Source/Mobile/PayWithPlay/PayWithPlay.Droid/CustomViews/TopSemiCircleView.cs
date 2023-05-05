using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using AndroidX.Core.Content;
using Org.W3c.Dom;
using PayWithPlay.Droid.Extensions;

namespace PayWithPlay.Droid.CustomViews
{
    public class TopSemiCircleView : FrameLayout
    {
        public static int SpecHeight;
        private Color _semiCircleColor;

        public TopSemiCircleView(Context? context) : base(context)
        {
        }

        public TopSemiCircleView(Context? context, IAttributeSet? attrs) : base(context, attrs)
        {
            Init(attrs);
        }

        public TopSemiCircleView(Context? context, IAttributeSet? attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init(attrs);
        }

        public TopSemiCircleView(Context? context, IAttributeSet? attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Init(attrs);
        }

        protected TopSemiCircleView(nint javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            if(SpecHeight == 0)
            {
                //SpecHeight = MeasureSpec.MakeMeasureSpec((int)(0.2284264f * this.Context!.Resources!.DisplayMetrics!.HeightPixels), MeasureSpecMode.Exactly);
                SpecHeight = MeasureSpec.MakeMeasureSpec((int)(0.48f * this.Context!.Resources!.DisplayMetrics!.WidthPixels), MeasureSpecMode.Exactly);
            }

            base.OnMeasure(widthMeasureSpec, SpecHeight);
        }

        public override void Draw(Canvas? canvas)
        {
            base.Draw(canvas);

            if (canvas == null)
            {
                return;
            }

            var paint = new Paint
            {
                Color = _semiCircleColor,
                AntiAlias = true
            };

            var startHeight = 2f.ToFloatPx();
            var offset = 1f.ToFloatPx();

            canvas.DrawRect(new RectF(0, 0, Width, startHeight), paint);
            canvas.DrawArc(new RectF(-offset, -(Height - startHeight), Width + offset, Height), 90f, 90f, true, paint);
            canvas.DrawArc(new RectF(-offset, -(Height - startHeight), Width + offset, Height), 0f, 90f, true, paint);
        }

        private void Init(IAttributeSet attributeSet)
        {
            var attrs = Context!.Theme!.ObtainStyledAttributes(attributeSet, Resource.Styleable.TopSemiCircleView, 0, 0);
            try
            {
                _semiCircleColor = attrs.GetColor(Resource.Styleable.TopSemiCircleView_semiCircleColor, new Color(ContextCompat.GetColor(Context, Resource.Color.secondary_color)));
            }
            finally
            {
                attrs.Recycle();
            }
        }
    }
}
