using MvvmCross.Platforms.Android.Binding.Target;

namespace PayWithPlay.Droid.CustomBindings.NumericKeybaordView
{
    public class NumericKeyboardFingerprintBinding : MvxAndroidTargetBinding<CustomViews.NumericKeybaordView, bool>
    {
        public const string Property = "FingerprintEnabled";

        public NumericKeyboardFingerprintBinding(CustomViews.NumericKeybaordView target) : base(target)
        {
        }

        protected override void SetValueImpl(CustomViews.NumericKeybaordView target, bool value)
        {
            if(target == null)
            {
                return;
            }

            target.SetFingerprintEnabled(value);
        }
    }
}
