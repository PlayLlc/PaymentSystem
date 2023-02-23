using AndroidX.AppCompat.Widget;
using MvvmCross.Platforms.Android.Binding.Target;
using PayWithPlay.Core.Enums;

namespace PayWithPlay.Droid.CustomBindings
{
    public class MerchantTypeBinding : MvxAndroidTargetBinding<AppCompatImageView, MerchantType>
    {
        public const string Property = "MerchantType";

        public MerchantTypeBinding(AppCompatImageView target) : base(target)
        {
        }

        protected override void SetValueImpl(AppCompatImageView target, MerchantType value)
        {
            if(target == null)
            {
                return;
            }

            switch (value)
            {
                case MerchantType.Business:
                    target.SetImageResource(Resource.Drawable.ic_merchant_business);
                    break;
                case MerchantType.NonProfit:
                    target.SetImageResource(Resource.Drawable.ic_merchant_non_profit);
                    break;
                case MerchantType.Individual:
                    target.SetImageResource(Resource.Drawable.ic_merchant_individual);
                    break;
            }
        }
    }
}