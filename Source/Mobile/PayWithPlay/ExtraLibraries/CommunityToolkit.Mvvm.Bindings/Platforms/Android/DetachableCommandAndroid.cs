using System;
using System.Reflection;
using System.Windows.Input;
using Android.Views;
using Android.Widget;

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

    public static EventToCommandInfo SetDetachableClickCommand(this View element, ICommand command)
    {
        return element.SetDetachableCommand(command, nameof(View.Click));
    }
}