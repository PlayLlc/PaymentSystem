using Google.Android.Material.Dialog;
using Microsoft.Maui.ApplicationModel;
using PayWithPlay.Core.Interfaces;

namespace PayWithPlay.Droid.Utils
{
    public class WheelPicker : IWheelPicker
    {
        public void Show(string[] values, int startIndex, string? title = null, string? positiveButtonTitle = null, string? negativeButtonTitle = null, Action<int>? onPositiveAction = null)
        {
            var currentActivity = Platform.CurrentActivity;
            if (currentActivity == null ||
                currentActivity.IsDestroyed ||
                currentActivity.IsFinishing)
            {
                return;
            }

            var builder = new MaterialAlertDialogBuilder(currentActivity);
            var view = currentActivity.LayoutInflater.Inflate(Resource.Layout.view_wheel_picker, null)!;
            builder.SetView(view);
            if (!string.IsNullOrWhiteSpace(title))
            {
                builder.SetTitle(title);
            }

            var numberPicker = view.FindViewById<NumberPicker>(Resource.Id.number_picker)!;

            numberPicker.MinValue = 0;
            numberPicker.MaxValue = values.Length - 1;
            numberPicker.SetDisplayedValues(values);
            numberPicker.WrapSelectorWheel = false;
            numberPicker.Value = startIndex;

            builder.SetPositiveButton(positiveButtonTitle, (sender, t) => 
            {
                onPositiveAction?.Invoke(numberPicker.Value);
            });

            if (!string.IsNullOrWhiteSpace(negativeButtonTitle))
            {
                builder.SetNegativeButton(negativeButtonTitle, (sender, t) => { });
            }

            builder.Create().Show();
        }
    }
}
