using MvvmCross.Platforms.Android.Binding.Target;

namespace PayWithPlay.Droid.CustomBindings.NumericKeyboardView
{
    public class NumericKeyboardFingerprintBinding : MvxAndroidTargetBinding<CustomViews.NumericKeyboardView, bool>
    {
        public const string Property = "FingerprintEnabled";

        public NumericKeyboardFingerprintBinding(CustomViews.NumericKeyboardView target) : base(target)
        {
        }

        protected override void SetValueImpl(CustomViews.NumericKeyboardView target, bool value)
        {
            if(target == null)
            {
                return;
            }

            target.SetFingerprintEnabled(value);
        }
    }
}
