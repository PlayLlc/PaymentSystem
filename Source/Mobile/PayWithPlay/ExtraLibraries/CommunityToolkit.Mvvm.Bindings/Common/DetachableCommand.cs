using System;
using System.Reflection;
using System.Windows.Input;

namespace CommunityToolkit.Mvvm.Bindings;

public static partial class DetachableCommand
{
    internal static EventToCommandInfo SetDetachableCommand(object element, string eventName, ICommand command)
    {
        var t = element.GetType();
        var e = t.GetEventInfoForControl(eventName);

        EventHandler handler = (s, args) =>
        {
            if (command.CanExecute(null))
            {
                command.Execute(null);
            }
        };

        var eventToCommandInfo = new EventToCommandInfo(element, eventName, handler, command);

        e.AddEventHandler(
            element,
            handler);

        AttachCanExecuteHandler(element, command, eventToCommandInfo);

        return eventToCommandInfo;
    }

    internal static void AttachCanExecuteHandler(object element, ICommand command, EventToCommandInfo eventToCommandInfo)
    {
        var t = element.GetType();
        var enabledProperty = t.GetProperty("Enabled");

        if (enabledProperty != null)
        {
            enabledProperty.SetValue(element, command.CanExecute(null));

            EventHandler canExecuteHandler = (s, args) =>
            {
                enabledProperty.SetValue(element, command.CanExecute(null));
            };

            command.CanExecuteChanged += canExecuteHandler;

            eventToCommandInfo.CanExecuteEventHandler = canExecuteHandler;
        }
    }
}

public class EventToCommandInfo
{
    private string _eventName;
    private WeakReference<object> _targetReference;
    private WeakReference<Delegate> _eventHandlerReference;

    private WeakReference<ICommand> _commandReference;
    private WeakReference<EventHandler> _canExecuteEventHandlerReference;

    internal EventToCommandInfo(object target, string eventName, Delegate eventHandler, ICommand command)
    {
        _eventName = eventName;
        _targetReference = new WeakReference<object>(target);
        _eventHandlerReference = new WeakReference<Delegate>(eventHandler);
        _commandReference = new WeakReference<ICommand>(command);

        _canExecuteEventHandlerReference = new WeakReference<EventHandler>(null);
    }

    internal EventHandler CanExecuteEventHandler
    {
        set => _canExecuteEventHandlerReference = new WeakReference<EventHandler>(value);
    }

    public void Detach()
    {
        if (_targetReference.TryGetTarget(out var target) &&
            _eventHandlerReference.TryGetTarget(out var eventHandler))
        {
            var t = target.GetType();
            var e = t.GetEventInfoForControl(_eventName);

            e.RemoveEventHandler(target, eventHandler);
        }

        if (_commandReference.TryGetTarget(out var command) &&
            _canExecuteEventHandlerReference.TryGetTarget(out var canExecuteEventHandler))
        {
            command.CanExecuteChanged -= canExecuteEventHandler;
        }
    }
}
