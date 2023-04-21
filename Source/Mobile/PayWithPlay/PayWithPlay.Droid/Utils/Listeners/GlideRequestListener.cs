using Bumptech.Glide.Load;
using Bumptech.Glide.Load.Engine;
using Bumptech.Glide.Request;
using Bumptech.Glide.Request.Target;
using Object = Java.Lang.Object;

namespace PayWithPlay.Droid.Utils.Listeners
{
    public class GlideRequestListener : Object, IRequestListener
    {
        private readonly Action<Object, Object, ITarget, DataSource, bool>? _onReadyAction;
        private readonly Action<GlideException, Object, ITarget, bool>? _onFailedAction;

        public GlideRequestListener(Action<Object, Object, ITarget, DataSource, bool>? onReadyAction = null, Action<GlideException, Object, ITarget, bool>? onFailedAction = null)
        {
            _onReadyAction = onReadyAction;
            _onFailedAction = onFailedAction;
        }

        public bool OnLoadFailed(GlideException glideException, Object model, ITarget target, bool isFirstResource)
        {
            _onFailedAction?.Invoke(glideException, model, target, isFirstResource);
            return false;
        }

        public bool OnResourceReady(Object resource, Object model, ITarget target, DataSource dataSource, bool isFirstResource)
        {
            _onReadyAction?.Invoke(resource, model, target, dataSource, isFirstResource);
            return false;
        }
    }
}
