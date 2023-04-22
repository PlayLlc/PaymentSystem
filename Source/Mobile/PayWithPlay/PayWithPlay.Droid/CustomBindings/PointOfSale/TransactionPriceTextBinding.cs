using Android.Graphics;
using AndroidX.Core.Content;
using MvvmCross.Platforms.Android.Binding.Target;

namespace PayWithPlay.Droid.CustomBindings.PointOfSale
{
    public class TransactionPriceTextBinding : MvxAndroidTargetBinding<TextView, bool>
    {
        public const string Property = "TransactionPriceText";

        public TransactionPriceTextBinding(TextView target) : base(target)
        {
        }

        protected override void SetValueImpl(TextView target, bool value)
        {
            if (target == null)
            {
                return;
            }

            if (value)
            {
                target.SetTextColor(new Color(ContextCompat.GetColor(target.Context, Resource.Color.negative_color)));
            }
            else
            {
                target.SetTextColor(new Color(ContextCompat.GetColor(target.Context, Resource.Color.primary_text_color)));
            }
        }
    }
}
