using Google.Android.Material.Button;
using MvvmCross.Platforms.Android.Binding.Target;

namespace PayWithPlay.Droid.CustomBindings
{
    public class DeviceSettingButtonBinding : MvxAndroidTargetBinding<MaterialButton, bool>
    {
        public const string Property = "DeviceSettingButtonBinding";

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
                target.SetIconResource(Resource.Drawable.ic_enable_settings_check);
                target.IconGravity = MaterialButton.IconGravityEnd;
                target.SetIconTintResource(Resource.Color.black);
            }
            else
            {
                target.Icon = null;
            }
        }
    }
}