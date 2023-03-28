using Android.Views;
using PayWithPlay.Droid.Utils;

namespace PayWithPlay.Droid.Extensions
{
    public static class ViewExtensions
    {
        public static void SetBackground(
            this View view,
            int colorResId,
            float? strokeWidth = null,
            int? strokeColorResId = null,
            float? cornerRadius = null)
        {
            view.Background = ViewUtils.GetShapeDrawable(colorResId, strokeWidth, strokeColorResId, cornerRadius, cornerRadius, cornerRadius, cornerRadius);
        }

        public static void SetBackground(
            this View view,
            int colorResId,
            float? strokeWidth = null,
            int? strokeColorResId = null,
            float? topLeft = null,
            float? topRight = null,
            float? bottomLeft = null,
            float? bottomRight = null)
        {
            view.Background = ViewUtils.GetShapeDrawable(colorResId, strokeWidth, strokeColorResId, topLeft, topRight, bottomLeft, bottomRight);
        }
    }
}
