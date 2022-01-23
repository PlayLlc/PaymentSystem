using System;
using System.Collections.Generic;

using Play.Random;

namespace Play.Events;

public abstract class EventHandlerBase<T> : IEquatable<EventHandlerBase<T>>, IEqualityComparer<EventHandlerBase<T>> where T : EventBase
{
    #region Instance Values

    protected readonly EventHandlerId _EventHandlerId;

    #endregion

    #region Constructor

    protected EventHandlerBase()
    {
        _EventHandlerId = new EventHandlerId(Randomize.Integers.UShort());
    }

    #endregion

    #region Instance Members

    public void Unsubscribe(IPlayEventBus eventBus) => eventBus.Unsubscribe(GetSubscriptionCorrelationId());
    public EventHandlerId GetEventHandlerId() => _EventHandlerId;
    public abstract EventTypeId GetEventTypeId();
    public abstract void Handle(EventBase @event); // where T : EventBase;
    public SubscriptionId GetSubscriptionCorrelationId() => new(_EventHandlerId, GetEventTypeId());

    #endregion

    #region Equality

    public override bool Equals(object? other) => other is EventHandlerBase<T> handler && Equals(handler);

    public bool Equals(EventHandlerBase<T>? other)
    {
        if (other == null)
            return false;

        return other._EventHandlerId == _EventHandlerId;
    }

    public bool Equals(EventHandlerBase<T>? x, EventHandlerBase<T>? y)
    {
        if (x == null)
            return y == null;

        if (y == null)
            return false;

        return x.Equals(y);
    }

    public override int GetHashCode() => 149 * _EventHandlerId;
    public int GetHashCode(EventHandlerBase<T> obj) => obj.GetHashCode();

    #endregion
}