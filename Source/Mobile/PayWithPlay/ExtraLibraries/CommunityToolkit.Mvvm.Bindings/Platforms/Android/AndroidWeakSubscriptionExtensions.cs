namespace CommunityToolkit.Mvvm.Bindings.Platforms.Android
{
    public static class AndroidWeakSubscriptionExtensions
    {
        public static JavaEventSubscription<TSource> WeakSubscribe<TSource>(this TSource source, string eventName, EventHandler eventHandler)
            where TSource : class
        {
            return new JavaEventSubscription<TSource>(source, eventName, eventHandler);
        }

        public static AndroidTargetEventSubscription<TSource, TEventArgs> WeakSubscribe<TSource, TEventArgs>(this TSource source, string eventName, EventHandler<TEventArgs> eventHandler)
            where TSource : class
        {
            return new AndroidTargetEventSubscription<TSource, TEventArgs>(source, eventName, eventHandler);
        }
    }
}
