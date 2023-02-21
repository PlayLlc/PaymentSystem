using Google.Android.Material.Button;
using MvvmCross.Platforms.Android.Binding.Target;

namespace PayWithPlay.Droid.CustomBindings
{
    public class UnderlineButtonTextBinding : MvxAndroidTargetBinding<MaterialButton, bool>
    {
        public const string Property = "Underline";

        public UnderlineButtonTextBinding(MaterialButton target) : base(target)
        {
        }

        protected override void SetValueImpl(MaterialButton target, bool value)
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
