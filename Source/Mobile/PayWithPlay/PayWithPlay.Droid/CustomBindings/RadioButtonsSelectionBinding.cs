using MvvmCross.Binding;
using MvvmCross.Platforms.Android.Binding.Target;
using PayWithPlay.Droid.CustomViews;
using System.ComponentModel;

namespace PayWithPlay.Droid.CustomBindings
{
    public class RadioButtonsSelectionBinding : MvxAndroidTargetBinding<RadioButtonsView, int?>
    {
        public const string Property = "RadioButtonSelectedType";
        private bool _subscribed;

        public RadioButtonsSelectionBinding(RadioButtonsView target) : base(target)
        {
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        protected override void SetValueImpl(RadioButtonsView target, int? value)
        {
            if(target == null)
            {
                return;
            }

            target.SelectedButtonType = value;
        }

        public override void SubscribeToEvents()
        {
            if (Target == null)
            {
                return;
            }

            Target.PropertyChanged += Target_PropertyChanged;
            _subscribed = true;
        }

        private void Target_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (Target == null)
            {
                return;
            }

            FireValueChanged(Target.SelectedButtonType);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                if (Target != null && _subscribed)
                {
                    Target.PropertyChanged -= Target_PropertyChanged;
                    _subscribed = false;
                }
            }
        }
    }
}
