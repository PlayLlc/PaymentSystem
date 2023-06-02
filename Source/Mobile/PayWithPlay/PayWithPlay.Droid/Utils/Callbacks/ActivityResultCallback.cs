using AndroidX.Activity.Result;

namespace PayWithPlay.Droid.Utils.Callbacks
{
    public class ActivityResultCallback : Java.Lang.Object, IActivityResultCallback
    {
        private readonly Action<Java.Lang.Object?>? _onResultAction;

        public ActivityResultCallback(Action<Java.Lang.Object?>? onResultAction)
        {
            this._onResultAction = onResultAction;
        }

        public void OnActivityResult(Java.Lang.Object? result)
        {
            _onResultAction?.Invoke(result);
        }
    }
}
