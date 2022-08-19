using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Play.Messaging.Threads;

namespace Play.Events;

public class PlayEventBus : CommandProcessingQueue<EventBase>, IPlayEventBus
{
    #region Instance Values

    private readonly Dictionary<EventTypeId, Dictionary<EventHandlerId, Action<EventBase>>> _HandlerMap;

    #endregion

    #region Constructor

    public PlayEventBus(CancellationTokenSource cancellationTokenSource) : base(cancellationTokenSource)
    {
        _HandlerMap = new Dictionary<EventTypeId, Dictionary<EventHandlerId, Action<EventBase>>>();
    }

    #endregion

    #region Instance Members

    protected override async Task Handle(EventBase @event)
    {
        if (!_HandlerMap.TryGetValue(@event.GetEventTypeId(), out Dictionary<EventHandlerId, Action<EventBase>> handlers))
            return;

        await Task.Run(() =>
        {
            foreach (Action<EventBase> handler in handlers.Values!)
                handler.Invoke(@event);
        }).ConfigureAwait(false);
    }

    public void Publish<T>(T @event) where T : EventBase
    {
        base.Enqueue(@event);
    }

    public void Subscribe<T>(EventHandlerBase<T> eventHandlerBase) where T : EventBase
    {
        if (!_HandlerMap.ContainsKey(eventHandlerBase.GetEventTypeId()))
            _HandlerMap.Add(eventHandlerBase.GetEventTypeId(), new Dictionary<EventHandlerId, Action<EventBase>>());
        _HandlerMap[eventHandlerBase.GetEventTypeId()].Add(eventHandlerBase.GetEventHandlerId(), eventHandlerBase.Handle);
    }

    public void Unsubscribe(SubscriptionId handler)
    {
        if (!_HandlerMap.ContainsKey(handler.GetEventTypeId()))
            return;

        if (!_HandlerMap[handler.GetEventTypeId()].ContainsKey(handler.GetEventHandlerId()))
            return;

        _HandlerMap[handler.GetEventTypeId()].Remove(handler.GetEventHandlerId());
    }

    #endregion
}