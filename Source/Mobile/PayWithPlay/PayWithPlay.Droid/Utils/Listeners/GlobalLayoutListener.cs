using Android.Views;

namespace PayWithPlay.Droid.Utils.Listeners
{
    public class GlobalLayoutListener : Java.Lang.Object, ViewTreeObserver.IOnGlobalLayoutListener
    {
        private readonly Action onGlobalLayout;

        public GlobalLayoutListener(Action onGlobalLayout)
        {
            this.onGlobalLayout = onGlobalLayout;
        }

        public void OnGlobalLayout()
        {
            onGlobalLayout?.Invoke();
        }
    }
}
