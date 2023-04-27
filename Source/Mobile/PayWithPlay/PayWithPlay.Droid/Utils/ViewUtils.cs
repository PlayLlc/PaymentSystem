using Android.Content.Res;
using Android.Graphics;
using AndroidX.Core.Content;
using Google.Android.Material.Shape;

namespace PayWithPlay.Droid.Utils
{
    public static class ViewUtils
    {
        public static MaterialShapeDrawable GetShapeDrawable(
            int colorResId,
            float? strokeWidth = null, 
            int? strokeColorResId = null, 
            float? topLeft = null,
            float? topRight = null, 
            float? bottomLeft = null, 
            float? bottomRight = null) 
        {
            ShapeAppearanceModel.Builder? backgroundShapeModel = null;
            if (topLeft != null)
            {
                backgroundShapeModel = new ShapeAppearanceModel.Builder();
                backgroundShapeModel.SetTopLeftCorner(CornerFamily.Rounded, topLeft.Value);
            }
            if (topRight != null)
            {
                backgroundShapeModel ??= new ShapeAppearanceModel.Builder();
                backgroundShapeModel.SetTopRightCorner(CornerFamily.Rounded, topRight.Value);
            }
            if (bottomLeft != null)
            {
                backgroundShapeModel ??= new ShapeAppearanceModel.Builder();
                backgroundShapeModel.SetBottomLeftCorner(CornerFamily.Rounded, bottomLeft.Value);
            }
            if (bottomRight != null)
            {
                backgroundShapeModel ??= new ShapeAppearanceModel.Builder();
                backgroundShapeModel.SetBottomRightCorner(CornerFamily.Rounded, bottomRight.Value);
            }

            MaterialShapeDrawable? drawable;
            if (backgroundShapeModel == null)
            {
                drawable = new MaterialShapeDrawable();
            }
            else
            {
                drawable = new MaterialShapeDrawable(backgroundShapeModel.Build());
            }

            drawable.FillColor = ColorStateList.ValueOf(new Color(ContextCompat.GetColor(App.Context, colorResId)));
            if (strokeWidth != null) 
            {
                drawable.StrokeWidth = strokeWidth.Value;
            }
            if (strokeColorResId != null) 
            {
                drawable.StrokeColor = ColorStateList.ValueOf(new Color(ContextCompat.GetColor(App.Context, strokeColorResId.Value)));
            }

            return drawable;
        }

        public static MaterialShapeDrawable GetShapeDrawable(
           ColorStateList color,
           float? strokeWidth = null,
           ColorStateList? strokeColor = null,
           float? topLeft = null,
           float? topRight = null,
           float? bottomLeft = null,
           float? bottomRight = null)
        {
            ShapeAppearanceModel.Builder? backgroundShapeModel = null;
            if (topLeft != null)
            {
                backgroundShapeModel = new ShapeAppearanceModel.Builder();
                backgroundShapeModel.SetTopLeftCorner(CornerFamily.Rounded, topLeft.Value);
            }
            if (topRight != null)
            {
                backgroundShapeModel ??= new ShapeAppearanceModel.Builder();
                backgroundShapeModel.SetTopRightCorner(CornerFamily.Rounded, topRight.Value);
            }
            if (bottomLeft != null)
            {
                backgroundShapeModel ??= new ShapeAppearanceModel.Builder();
                backgroundShapeModel.SetBottomLeftCorner(CornerFamily.Rounded, bottomLeft.Value);
            }
            if (bottomRight != null)
            {
                backgroundShapeModel ??= new ShapeAppearanceModel.Builder();
                backgroundShapeModel.SetBottomRightCorner(CornerFamily.Rounded, bottomRight.Value);
            }

            MaterialShapeDrawable? drawable;
            if (backgroundShapeModel == null)
            {
                drawable = new MaterialShapeDrawable();
            }
            else
            {
                drawable = new MaterialShapeDrawable(backgroundShapeModel.Build());
            }

            drawable.FillColor = color;
            if (strokeWidth != null)
            {
                drawable.StrokeWidth = strokeWidth.Value;
            }
            if (strokeColor != null)
            {
                drawable.StrokeColor = strokeColor;
            }

            return drawable;
        }
    }
}
