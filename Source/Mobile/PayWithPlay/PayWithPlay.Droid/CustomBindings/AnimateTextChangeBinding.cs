using Android.Animation;
using MvvmCross.Platforms.Android.Binding.Target;

namespace PayWithPlay.Droid.CustomBindings
{
    public class AnimateTextChangeBinding : MvxAndroidTargetBinding<TextView, string>
    {
        public const string Property = "AnimateText";

        public AnimateTextChangeBinding(TextView target) : base(target)
        {
        }

        protected override void SetValueImpl(TextView target, string value)
        {
            if (target == null) 
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(target.Text))
            {
                target.Text = value;
            }
            else 
            {
                float initialScale = 1.0f;
                float finalScale = 1.4f;
                var scaleAnimator = ObjectAnimator.OfPropertyValuesHolder(
                    target, 
                    PropertyValuesHolder.OfFloat("scaleX", initialScale, finalScale, initialScale),
                    PropertyValuesHolder.OfFloat("scaleY", initialScale, finalScale, initialScale));
                scaleAnimator.SetAutoCancel(true);
                scaleAnimator.SetDuration(500);
                 
                target.Text = value;

                scaleAnimator.Start();
            }
        }
    }
}
