using Android.Graphics;
using AndroidX.Core.Content;
using MvvmCross.Platforms.Android.Binding.Target;

namespace PayWithPlay.Droid.CustomBindings.Inventory
{
    public class InventoryItemPriceTextColorBinding : MvxAndroidTargetBinding<TextView, bool>
    {
        public const string Property = "ItemPriceTextColor";

        public InventoryItemPriceTextColorBinding(TextView target) : base(target)
        {
        }

        protected override void SetValueImpl(TextView target, bool hasDiscounts)
        {
            if (target == null)
            {
                return;
            }

            if (hasDiscounts)
            {
                target.SetTextColor(new Color(ContextCompat.GetColor(target.Context, Resource.Color.accent_color)));
            }
            else
            {
                target.SetTextColor(new Color(ContextCompat.GetColor(target.Context, Resource.Color.primary_text_color)));
            }
        }
    }
}
