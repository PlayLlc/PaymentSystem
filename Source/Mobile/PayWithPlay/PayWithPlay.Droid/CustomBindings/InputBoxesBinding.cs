using MvvmCross.Binding;
using MvvmCross.Platforms.Android.Binding.Target;
using PayWithPlay.Droid.CustomViews;

namespace PayWithPlay.Droid.CustomBindings
{
    public class InputBoxesBinding : MvxAndroidTargetBinding<InputBoxesView, string>
    {
        public const string Property = "InputBoxesValue";


        private bool _subscribed;

        public InputBoxesBinding(InputBoxesView target) : base(target)
        {
        }

        protected override void SetValueImpl(InputBoxesView target, string value)
        {
            if(target == null)
            {
                return;
            }

            target.SetTextValue(value);
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public override void SubscribeToEvents()
        {
            if(Target == null)
            {
                return;
            }

            Target.PropertyChanged += Target_PropertyChanged;
            _subscribed = true;
        }

        private void Target_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (Target == null)
            {
                return;
            }

            FireValueChanged(Target.TextValue);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                if(Target != null && _subscribed)
                {
                    Target.PropertyChanged -= Target_PropertyChanged;
                    _subscribed = false;
                }
            }
        }
    }
}