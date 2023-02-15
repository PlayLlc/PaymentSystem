using Android.Runtime;
using CommunityToolkit.Mvvm.Bindings.Common;
using System.Reflection;

namespace CommunityToolkit.Mvvm.Bindings.Platforms.Android
{
    /// <summary>
    /// Weak subscription to an event where the target may be an IJavaObject
    /// and could be collected by the Android runtime before being collected by the Mono GC.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TEventArgs"></typeparam>
    public class AndroidTargetEventSubscription<TSource, TEventArgs> : WeakEventSubscription<TSource, TEventArgs>
        where TSource : class
    {
        public AndroidTargetEventSubscription(TSource source, string sourceEventName, EventHandler<TEventArgs> targetEventHandler)
            : base(source, sourceEventName, targetEventHandler)
        {
        }

        public AndroidTargetEventSubscription(TSource source, EventInfo sourceEventInfo, EventHandler<TEventArgs> targetEventHandler)
            : base(source, sourceEventInfo, targetEventHandler)
        {
        }

        protected override object GetTargetObject()
        {
            // If the object has been GCed by java but NOT mono
            // then it is invalid and should not be manipulated.
            var target = base.GetTargetObject();
            var javaObj = target as IJavaObject;
            if (javaObj != null && javaObj.Handle == IntPtr.Zero)
            {
                return null;
            }
            return target;
        }

        protected override Delegate CreateEventHandler()
        {
            return new EventHandler<TEventArgs>(OnSourceEvent);
        }
    }

    public class JavaEventSubscription<TSource> : WeakEventSubscription<TSource> where TSource : class
    {
        public JavaEventSubscription(TSource source, string sourceEventName, EventHandler targetEventHandler)
            : base(source, sourceEventName, targetEventHandler)
        {
        }

        public JavaEventSubscription(TSource source, EventInfo sourceEventInfo, EventHandler targetEventHandler)
            : base(source, sourceEventInfo, targetEventHandler)
        {
        }

        protected override object GetTargetObject()
        {
            // If the object has been GCed by java but NOT mono
            // then it is invalid and should not be manipulated.
            var target = base.GetTargetObject();
            var javaObj = target as IJavaObject;
            if (javaObj != null && javaObj.Handle == IntPtr.Zero)
            {
                return null;
            }
            return target;
        }

        protected override Delegate CreateEventHandler()
        {
            return new EventHandler(OnSourceEvent);
        }
    }
}