namespace Play.Events;

public interface IPlayEventBus
{
    #region Instance Members

    public void Publish<T>(T @event) where T : EventBase;
    public void Subscribe<T>(EventHandlerBase<T> eventHandlerBase) where T : EventBase;
    public void Unsubscribe(SubscriptionId subscriptionId);

    #endregion
}