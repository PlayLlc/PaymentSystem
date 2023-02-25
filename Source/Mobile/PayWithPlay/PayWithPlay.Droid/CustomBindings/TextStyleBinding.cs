using AndroidX.AppCompat.Widget;
using MvvmCross.Platforms.Android.Binding.Target;
using PayWithPlay.Core.Enums;

namespace PayWithPlay.Droid.CustomBindings
{
    public class TextStyleBinding : MvxAndroidTargetBinding<TextView, TextStyleType>
    {
        public const string Property = "TextStyle";

        public TextStyleBinding(TextView target) : base(target)
        {
        }

        protected override void SetValueImpl(TextView target, TextStyleType styleType)
        {
            if(target == null)
            {
                return;
            }

            switch(styleType) 
            {
                case TextStyleType.BigTitle:
                    target.SetTextAppearance(Resource.Style.BigTitleStyle);
                    break;
                case TextStyleType.MediumTitle:
                    target.SetTextAppearance(Resource.Style.MediumTitleStyle);
                    break;
            }
        }
    }
}
