using Android.Content.Res;
using Android.Graphics;
using Android.Util;
using Android.Views;
using AndroidX.Core.Content;
using PayWithPlay.Droid.Utils;
using static Android.Views.ViewGroup;

namespace PayWithPlay.Droid.Extensions
{
    public static class ViewExtensions
    {
        public static void SetSize(this View view, int height = int.MinValue, int width = int.MinValue)
        {
            if (view == null ||
                (height == int.MinValue && width == int.MinValue))
            {
                return;
            }

            var viewLp = view.LayoutParameters;
            if (height != int.MinValue)
            {
                viewLp!.Height = height;
            }
            if (width != int.MinValue)
            {
                viewLp!.Width = width;
            }
            view.LayoutParameters = viewLp;
        }

        public static void SetMargins(this View view, int start = int.MinValue, int top = int.MinValue, int end = int.MinValue, int bottom = int.MinValue)
        {
            if (view == null ||
               (start == int.MinValue && top == int.MinValue && end == int.MinValue && bottom == int.MinValue))
            {
                return;
            }

            var viewLp = view.LayoutParameters as MarginLayoutParams;
            if (start != int.MinValue)
            {
                viewLp!.MarginStart = start;
            }
            if (top != int.MinValue)
            {
                viewLp!.TopMargin = top;
            }
            if (end != int.MinValue)
            {
                viewLp!.MarginEnd = end;
            }
            if (bottom != int.MinValue)
            {
                viewLp!.BottomMargin = bottom;
            }
            view.LayoutParameters = viewLp;
        }

        public static void SetBackground(
            this View view,
            ColorStateList color,
            float? strokeWidth = null,
            ColorStateList? strokeColor = null,
            float? cornerRadius = null)
        {
            view.Background = ViewUtils.GetShapeDrawable(color, strokeWidth, strokeColor, cornerRadius, cornerRadius, cornerRadius, cornerRadius);
        }

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

        public static void InitShapeableView(this View view, IAttributeSet attributeSet)
        {
            var attrs = view.Context!.Theme!.ObtainStyledAttributes(attributeSet, Resource.Styleable.ShapeableView, 0, 0);

            Color fillColor;
            Color borderColor;
            int cornerRadius;
            int borderWidth;
            try
            {
                fillColor = attrs.GetColor(Resource.Styleable.ShapeableView_fillColor, new Color(ContextCompat.GetColor(view.Context, Resource.Color.secondary_color)));
                borderColor = attrs.GetColor(Resource.Styleable.ShapeableView_borderColor, -1);
                cornerRadius = attrs.GetDimensionPixelSize(Resource.Styleable.ShapeableView_cornerRadius, 0);
                borderWidth = attrs.GetDimensionPixelSize(Resource.Styleable.ShapeableView_borderWidth, -1);
            }
            finally
            {
                attrs.Recycle();
            }

            view.SetBackground(ColorStateList.ValueOf(fillColor), borderWidth == -1 ? null : borderWidth, borderColor == -1 ? null : ColorStateList.ValueOf(borderColor), cornerRadius: cornerRadius);
        }
    }
}
