using Google.Android.Material.Button;
using MvvmCross.Platforms.Android.Binding.Target;
using PayWithPlay.Android.CustomViews;

namespace PayWithPlay.Android.CustomBindings
{
    public class DeviceSettingButtonBinding : MvxAndroidTargetBinding<MaterialButton, bool>
    {
        public string Property = "DeviceSettingButtonBinding";

        public DeviceSettingButtonBinding(MaterialButton target) : base(target)
        {
        }

        protected override void SetValueImpl(MaterialButton target, bool value)
        {
            if(target == null)
            {
                return;
            }

            if (value)
            {
                //target.SetIconResource
            }
        }
    }
}
