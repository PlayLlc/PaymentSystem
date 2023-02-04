using Android.Text.Style;
using Android.Views;

namespace PayWithPlay.Android.Utils
{
    public class CustomClickableSpan : ClickableSpan
    {
        private readonly Action<View> _action;

        public CustomClickableSpan(Action<View> action)
        {
            _action = action;
        }

        public override void OnClick(View widget)
        {
            _action?.Invoke(widget);
        }
    }
}