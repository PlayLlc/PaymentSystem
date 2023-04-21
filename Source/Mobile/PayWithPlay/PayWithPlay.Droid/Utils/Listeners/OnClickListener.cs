using Android.Views;
using static Android.Views.View;

namespace PayWithPlay.Droid.Utils.Listeners
{
    public class OnClickListener : Java.Lang.Object, IOnClickListener
    {
        private Action<View> _onClickAction;

        public OnClickListener(Action<View> onClickAction)
        {
            _onClickAction = onClickAction;
        }

        public void OnClick(View? v)
        {
            _onClickAction?.Invoke(v);
        }
    }
}
