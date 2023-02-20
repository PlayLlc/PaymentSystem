using Google.Android.Material.Button;
using Google.Android.Material.TextField;
using MvvmCross.Platforms.Android.Binding.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayWithPlay.Droid.CustomBindings
{
    public class SetErrorInputBinding : MvxAndroidTargetBinding<TextInputLayout, string>
    {
        public const string Property = "InputError";

        public SetErrorInputBinding(TextInputLayout target) : base(target)
        {
        }

        protected override void SetValueImpl(TextInputLayout target, string value)
        {
            if(target == null)
            {
                return;
            }

            target.ErrorEnabled = !string.IsNullOrWhiteSpace(value);
            target.Error = value;
        }
    }
}
