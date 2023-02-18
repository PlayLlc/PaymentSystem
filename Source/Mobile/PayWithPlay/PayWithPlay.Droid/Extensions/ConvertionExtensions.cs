using Android.Util;

namespace PayWithPlay.Droid.Extensions
{
    public static class ConvertionExtensions
    {
        public static int ToPx(this float value)
        {
            return (int)ToFloatPx(value);
        }

        public static float ToFloatPx(this float value)
        {
            return TypedValue.ApplyDimension(ComplexUnitType.Dip, value, App.Context.Resources!.DisplayMetrics);
        }
    }
}
