using System.Reflection;

namespace CommunityToolkit.Mvvm.Bindings.Common
{
    public class WeakEventSubscription<TSource, TEventArgs> : IDisposable
            where TSource : class
    {
        private readonly WeakReference _targetReference;
        private readonly WeakReference<TSource> _sourceReference;

        private readonly MethodInfo _eventHandlerMethodInfo;

        private readonly EventInfo _sourceEventInfo;

        // we store a copy of our Delegate/EventHandler in order to prevent it being
        // garbage collected while the `client` still has ownership of this subscription
        private readonly Delegate _ourEventHandler;

        private bool _subscribed;

        public WeakEventSubscription(
            TSource source,
            string sourceEventName,
            EventHandler<TEventArgs> targetEventHandler)
            : this(source, typeof(TSource).GetEvent(sourceEventName), targetEventHandler)
        {
        }

        protected WeakEventSubscription(
            TSource source,
            EventInfo sourceEventInfo,
            EventHandler<TEventArgs> targetEventHandler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), "missing source in WeakEventSubscription");

            if (sourceEventInfo == null)
                throw new ArgumentNullException(nameof(sourceEventInfo),
                                                "missing source event info in WeakEventSubscription");

            _eventHandlerMethodInfo = targetEventHandler.GetMethodInfo();
            _targetReference = new WeakReference(targetEventHandler.Target);
            _sourceReference = new WeakReference<TSource>(source);
            _sourceEventInfo = sourceEventInfo;

            _ourEventHandler = Init();

            AddEventHandler();
        }

        private Delegate Init()
        {
            return CreateEventHandler();
        }

        protected virtual Delegate CreateEventHandler()
        {
            return new EventHandler<TEventArgs>(OnSourceEvent);
        }

        protected virtual object GetTargetObject()
        {
            return _targetReference.Target;
        }

        //This is the method that will handle the event of source.
        protected void OnSourceEvent(object sender, TEventArgs e)
        {
            var target = GetTargetObject();
            if (target != null)
            {
                _eventHandlerMethodInfo.Invoke(target, new[] { sender, e });
            }
            else
            {
                RemoveEventHandler();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                RemoveEventHandler();
            }
        }

        private void RemoveEventHandler()
        {
            if (!_subscribed)
                return;

            try
            {
                TSource source;
                if (_sourceReference.TryGetTarget(out source))
                {
                    _sourceEventInfo.GetRemoveMethod().Invoke(source, new object[] { _ourEventHandler });
                    _subscribed = false;
                }
            }
            catch (TargetInvocationException tie) when (tie.InnerException is ObjectDisposedException)
            {
                // we don't care if source has already been disposed
                _subscribed = false;
            }
        }

        private void AddEventHandler()
        {
            if (_subscribed)
                throw new Exception("Should not call AddEventHandler twice");

            TSource source;
            if (_sourceReference.TryGetTarget(out source))
            {
                _sourceEventInfo.GetAddMethod().Invoke(source, new object[] { _ourEventHandler });
                _subscribed = true;
            }
        }
    }

    public class WeakEventSubscription<TSource> : IDisposable
        where TSource : class
    {
        private readonly WeakReference _targetReference;
        private readonly WeakReference<TSource> _sourceReference;

        private readonly MethodInfo _eventHandlerMethodInfo;

        private readonly EventInfo _sourceEventInfo;

        // we store a copy of our Delegate/EventHandler in order to prevent it being
        // garbage collected while the `client` still has ownership of this subscription
        private readonly Delegate _ourEventHandler;

        private bool _subscribed;

        public WeakEventSubscription(
            TSource source,
            string sourceEventName,
            EventHandler targetEventHandler)
            : this(source, typeof(TSource).GetEvent(sourceEventName), targetEventHandler)
        {
        }

        protected WeakEventSubscription(
            TSource source,
            EventInfo sourceEventInfo,
            EventHandler targetEventHandler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), "missing source in WeakEventSubscription");

            if (sourceEventInfo == null)
                throw new ArgumentNullException(nameof(sourceEventInfo),
                                                "missing source event info in WeakEventSubscription");

            _eventHandlerMethodInfo = targetEventHandler.GetMethodInfo();
            _targetReference = new WeakReference(targetEventHandler.Target);
            _sourceReference = new WeakReference<TSource>(source);
            _sourceEventInfo = sourceEventInfo;

            // TODO: need to move this virtual call out of the constructor - need to implement a separate Init() method
            _ourEventHandler = CreateEventHandler();

            AddEventHandler();
        }

        protected virtual object GetTargetObject()
        {
            return _targetReference.Target;
        }

        protected virtual Delegate CreateEventHandler()
        {
            return new EventHandler(OnSourceEvent);
        }

        //This is the method that will handle the event of source.
        protected void OnSourceEvent(object sender, EventArgs e)
        {
            var target = GetTargetObject();
            if (target != null)
            {
                _eventHandlerMethodInfo.Invoke(target, new[] { sender, e });
            }
            else
            {
                RemoveEventHandler();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                RemoveEventHandler();
            }
        }

        private void RemoveEventHandler()
        {
            if (!_subscribed)
                return;

            try
            {
                TSource source;
                if (_sourceReference.TryGetTarget(out source))
                {
                    _sourceEventInfo.GetRemoveMethod().Invoke(source, new object[] { _ourEventHandler });
                    _subscribed = false;
                }
            }
            catch (TargetInvocationException tie) when (tie.InnerException is ObjectDisposedException)
            {
                // we don't care if source has already been disposed
                _subscribed = false;
            }
        }

        private void AddEventHandler()
        {
            if (_subscribed)
                throw new Exception("Should not call AddEventHandler() twice");

            TSource source;
            if (_sourceReference.TryGetTarget(out source))
            {
                _sourceEventInfo.GetAddMethod().Invoke(source, new object[] { _ourEventHandler });
                _subscribed = true;
            }
        }
    }
}
