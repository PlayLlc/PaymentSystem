using Android.Content;
using Android.Graphics;
using Android.Hardware.Lights;
using Android.Runtime;
using Android.Util;
using Android.Views;
using AndroidX.Core.Content;
using PayWithPlay.Droid.Extensions;

namespace PayWithPlay.Droid.CustomViews
{
    public class TopSemiCircleView : FrameLayout
    {
        public static int SpecHeight;

        public TopSemiCircleView(Context? context) : base(context)
        {
        }

        public TopSemiCircleView(Context? context, IAttributeSet? attrs) : base(context, attrs)
        {
        }

        public TopSemiCircleView(Context? context, IAttributeSet? attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public TopSemiCircleView(Context? context, IAttributeSet? attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        protected TopSemiCircleView(nint javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            if(SpecHeight == 0)
            {
                SpecHeight = MeasureSpec.MakeMeasureSpec((int)(0.2216f * this.Context!.Resources!.DisplayMetrics!.HeightPixels), MeasureSpecMode.Exactly);
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
                Color = new Color(ContextCompat.GetColor(Context, Resource.Color.secondary_color)),
                AntiAlias = true
            };

            var startHeight = 2f.ToFloatPx();
            var offset = 1f.ToFloatPx();

            canvas.DrawRect(new RectF(0, 0, Width, startHeight), paint);
            canvas.DrawArc(new RectF(-offset, -(Height - startHeight), Width + offset, Height), 90f, 90f, true, paint);
            canvas.DrawArc(new RectF(-offset, -(Height - startHeight), Width + offset, Height), 0f, 90f, true, paint);
        }
     }
}
