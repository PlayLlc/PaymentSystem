using Android.Graphics;
using Android.Text.Style;
using Android.Text;

namespace PayWithPlay.Droid.Utils
{
    public class CustomTypefaceSpan : TypefaceSpan
    {
        private readonly Typeface _newType;

        public CustomTypefaceSpan(string family, Typeface type) : base(family)
        {
            _newType = type;
        }

        public CustomTypefaceSpan(Typeface type) : base(string.Empty)
        {
            _newType = type;
        }

        public override void UpdateDrawState(TextPaint ds)
        {
            base.UpdateDrawState(ds);
            ApplyCustomTypeFace(ds, _newType);
        }

        public override void UpdateMeasureState(TextPaint paint)
        {
            base.UpdateMeasureState(paint);
            ApplyCustomTypeFace(paint, _newType);
        }

        private void ApplyCustomTypeFace(Paint paint, Typeface tf)
        {
            TypefaceStyle oldStyle;
            var old = paint.Typeface;
            if (old == null)
            {
                oldStyle = 0;
            }
            else
            {
                oldStyle = old.Style;
            }

            var fake = oldStyle & ~tf.Style;
            if ((fake & TypefaceStyle.Bold) != 0)
            {
                paint.FakeBoldText = true;
            }

            if ((fake & TypefaceStyle.Italic) != 0)
            {
                paint.TextSkewX = -0.25f;
            }

            paint.SetTypeface(tf);
        }
    }
}