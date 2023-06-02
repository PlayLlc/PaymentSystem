using Google.Android.Material.Button;
using MvvmCross.Platforms.Android.Binding.Target;

namespace PayWithPlay.Droid.CustomBindings
{
    public class UnderlineTextBinding : MvxAndroidTargetBinding<TextView, bool>
    {
        public const string Property = "Underline";

        public UnderlineTextBinding(TextView target) : base(target)
        {
        }

        protected override void SetValueImpl(TextView target, bool value)
        {
            if(target == null)
            {
                return;
            }

            target.PaintFlags |= Android.Graphics.PaintFlags.UnderlineText;
            target.SetLinkTextColor(target.TextColors);
            target.SetTextColor(target.TextColors);
        }
    }
}
