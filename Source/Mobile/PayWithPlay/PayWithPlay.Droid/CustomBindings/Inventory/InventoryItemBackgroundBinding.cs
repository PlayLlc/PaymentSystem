using Android.Content.Res;
using Android.Graphics;
using Android.Views;
using AndroidX.Core.Content;
using Google.Android.Material.Shape;
using MvvmCross.Platforms.Android.Binding.Target;
using PayWithPlay.Droid.Extensions;

namespace PayWithPlay.Droid.CustomBindings.Inventory
{
    public class InventoryItemBackgroundBinding : MvxAndroidTargetBinding<View, bool>
    {
        public const string Property = "InventoryItemBackground";

        public InventoryItemBackgroundBinding(View target) : base(target)
        {
        }

        protected override void SetValueImpl(View target, bool hasDiscount)
        {
            if (target == null)
            {
                return;
            }

            if (hasDiscount)
            {
                var strokeColorResId = Resource.Color.accent_color;

                if (target.Background is MaterialShapeDrawable materialShapeDrawable)
                {
                    materialShapeDrawable.StrokeColor = ColorStateList.ValueOf(new Color(ContextCompat.GetColor(App.Context, strokeColorResId)));
                    materialShapeDrawable.StrokeWidth = 2f.ToFloatPx();
                }
                else
                {
                    target.SetBackground(Resource.Color.third_color, 2f.ToFloatPx(), strokeColorResId, 5f.ToFloatPx());
                }
            }
            else
            {
                if (target.Background is MaterialShapeDrawable materialShapeDrawable)
                {
                    materialShapeDrawable.StrokeColor = ColorStateList.ValueOf(Color.Transparent);
                    materialShapeDrawable.StrokeWidth = 0f.ToFloatPx();
                }
                else
                {
                    target.SetBackground(Resource.Color.third_color, 0f, Resource.Color.transparent, 5f.ToFloatPx());
                }
            }
        }
    }
}
