using Android.Content;
using Android.Views;
using AndroidX.AsyncLayoutInflater.View;

namespace PayWithPlay.Droid.Utils
{
    public class InflateUtil
    {
        public static void InflateAsync(Context context, int resourceId, AsyncLayoutInflater.IOnInflateFinishedListener onInflateFinishedListener)
        {
            var asyncInflater = new AsyncLayoutInflater(context);
            asyncInflater.Inflate(resourceId, null, onInflateFinishedListener);
        }

        public static void InflateAsync(Context context, int resourceId, Action<View> onFinishInflate)
        {
            var asyncInflater = new AsyncLayoutInflater(context);
            asyncInflater.Inflate(resourceId, null, new InflateAsyncFinishedListener(onFinishInflate));
        }

        public static Task<View> InflateAsync(Context context, int resourceId)
        {
            var tcs = new TaskCompletionSource<View>();
            var listener = new AsyncInflateFinishedListener(tcs);
            Application.SynchronizationContext.Post(_ =>
            {
                var asyncInflater = new AsyncLayoutInflater(context);
                asyncInflater.Inflate(resourceId, null, listener);
            }, null);

            return tcs.Task;
        }

        private class AsyncInflateFinishedListener : Java.Lang.Object, AsyncLayoutInflater.IOnInflateFinishedListener
        {
            private readonly TaskCompletionSource<View> _tcs;

            public AsyncInflateFinishedListener(TaskCompletionSource<View> tcs)
            {
                _tcs = tcs;
            }

            public void OnInflateFinished(View? view, int resid, ViewGroup? parent)
            {
                _tcs.TrySetResult(view);
            }
        }
    }

    public class InflateAsyncFinishedListener : Java.Lang.Object, AsyncLayoutInflater.IOnInflateFinishedListener
    {
        private readonly Action<View> mOnFinishInflate;

        public InflateAsyncFinishedListener(Action<View> onFinishInflate)
        {
            mOnFinishInflate = onFinishInflate;
        }

        public void OnInflateFinished(View? view, int resid, ViewGroup? parent)
        {
            mOnFinishInflate?.Invoke(view);
        }
    }
}
