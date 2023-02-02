// ****************************************************************************
// <copyright file="ExtensionsApple.cs" company="GalaSoft Laurent Bugnion">
// Copyright © GalaSoft Laurent Bugnion 2009-2016
// </copyright>
// ****************************************************************************
// <author>Laurent Bugnion</author>
// <email>laurent@galasoft.ch</email>
// <date>19.01.2016</date>
// <project>GalaSoft.MvvmLight</project>
// <web>http://www.mvvmlight.net</web>
// <license>
// See license.txt in this solution or http://www.galasoft.ch/license_MIT.txt
// </license>
// ****************************************************************************

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Foundation;
using UIKit;

namespace CommunityToolkit.Mvvm.Bindings;

/// <summary>
/// Defines extension methods for iOS only.
/// </summary>
public static class ExtensionsApple
{
    internal static string GetDefaultEventNameForControl(this Type type)
    {
        string eventName = null;

        if (type == typeof (UIButton)
            || typeof (UIButton).IsAssignableFrom(type))
        {
            eventName = "TouchUpInside";
        }
        else if (type == typeof (UIBarButtonItem)
                 || typeof (UIBarButtonItem).IsAssignableFrom(type))
        {
            eventName = "Clicked";
        }
        else if (type == typeof (UISwitch)
                 || typeof (UISwitch).IsAssignableFrom(type))
        {
            eventName = "ValueChanged";
        }

        return eventName;
    }


    internal static Delegate GetCommandHandler(
        this EventInfo info,
        string eventName,
        Type elementType,
        ICommand command)
    {
        // At the moment, all supported controls with default events
        // in iOS are using EventHandler, and not EventHandler<...>.

        EventHandler handler = (s, args) =>
        {
            if (command.CanExecute(null))
            {
                command.Execute(null);
            }
        };

        return handler;
    }

    internal static Delegate GetCommandHandler<T>(
        this EventInfo info,
        string eventName,
        Type elementType,
        RelayCommand<T> command,
        Binding<T, T> castedBinding)
    {
        // At the moment, all supported controls with default events
        // in iOS are using EventHandler, and not EventHandler<...>.

        EventHandler handler = (s, args) =>
        {
            var param = castedBinding == null ? default(T) : castedBinding.Value;
            command.Execute(param);
        };

        return handler;
    }

    internal static Delegate GetCommandHandler<T>(
        this EventInfo info,
        string eventName,
        Type elementType,
        RelayCommand<T> command,
        T commandParameter)
    {
        // At the moment, all supported controls with default events
        // in iOS are using EventHandler, and not EventHandler<...>.

        EventHandler handler = (s, args) => command.Execute(commandParameter);
        return handler;
    }
}