using MvvmCross.Platforms.Android.Binding.Target;

namespace PayWithPlay.Droid.CustomBindings
{
    public class StrikethruTextBinding : MvxAndroidTargetBinding<TextView, bool>
    {
        public const string Property = "Strikethru";

        public StrikethruTextBinding(TextView target) : base(target)
        {
        }

        protected override void SetValueImpl(TextView target, bool value)
        {
            if (target == null)
            {
                return;
            }

            target.PaintFlags |= Android.Graphics.PaintFlags.StrikeThruText;
        }
    }
}
