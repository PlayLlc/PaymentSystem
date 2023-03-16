using Google.Android.Material.Button;
using MvvmCross.Platforms.Android.Binding.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayWithPlay.Droid.CustomBindings
{
    public class ButtonWithIconCenterTextBinding : MvxAndroidTargetBinding<MaterialButton, bool>
    {
        public const string Property = "CenterTextWhenIcon";

        public ButtonWithIconCenterTextBinding(MaterialButton target) : base(target)
        {
        }

        protected override void SetValueImpl(MaterialButton target, bool value)
        {
            if (target == null)
            {
                return;
            }

            target.IconPadding = -(target.Icon?.IntrinsicWidth ?? 0);
        }
    }
}
