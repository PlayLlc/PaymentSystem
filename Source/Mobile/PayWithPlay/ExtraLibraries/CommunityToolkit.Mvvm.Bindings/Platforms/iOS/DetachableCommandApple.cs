using System;
using System.Reflection;
using System.Windows.Input;
using UIKit;

namespace CommunityToolkit.Mvvm.Bindings;

public static partial class DetachableCommand
{
    public static EventToCommandInfo SetDetachableCommand(this object element, ICommand command, string eventName = null)
    {
        if (string.IsNullOrEmpty(eventName))
        {
            eventName = element.GetType().GetDefaultEventNameForControl();
        }

        return SetDetachableCommand(element, eventName, command);
    }
}