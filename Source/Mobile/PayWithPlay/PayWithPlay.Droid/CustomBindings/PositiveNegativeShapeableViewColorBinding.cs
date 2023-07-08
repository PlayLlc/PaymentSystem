using Android.Content.Res;
using Android.Graphics;
using AndroidX.Core.Content;
using Google.Android.Material.Shape;
using MvvmCross.Platforms.Android.Binding.Target;
using PayWithPlay.Droid.CustomViews.ShapeableViews;

namespace PayWithPlay.Droid.CustomBindings
{
    public class PositiveNegativeShapeableViewColorBinding : MvxAndroidTargetBinding<ShapeableView, bool>
    {
        public const string Property = "PositiveNegativeBgColor";

        public PositiveNegativeShapeableViewColorBinding(ShapeableView target) : base(target)
        {
        }

        protected override void SetValueImpl(ShapeableView target, bool value)
        {
            if (target == null)
            {
                return;
            }

            var drawable = (MaterialShapeDrawable)target.Background!;
            if (value)
            {
                drawable.FillColor = ColorStateList.ValueOf(new Color(ContextCompat.GetColor(target.Context, Resource.Color.positive_color)));
            }
            else
            {
                drawable.FillColor = ColorStateList.ValueOf(new Color(ContextCompat.GetColor(target.Context, Resource.Color.negative_color)));
            }
        }
    }
}
