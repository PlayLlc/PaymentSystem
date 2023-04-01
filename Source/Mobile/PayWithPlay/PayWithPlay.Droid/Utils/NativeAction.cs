using Kotlin.Jvm.Functions;

namespace PayWithPlay.Droid.Utils
{
    public class NativeAction : Java.Lang.Object, IFunction1
    {
        private readonly Action<Java.Lang.Object> _action;

        public NativeAction(Action<Java.Lang.Object> action)
        {
            _action = action;
        }

        public Java.Lang.Object? Invoke(Java.Lang.Object? p0)
        {
            _action?.Invoke(p0);

            return null;
        }
    }
}
