﻿using System;

namespace Play.Globalization.Time;

public class TimeoutManager : IDisposable
{
    #region Instance Values

    private readonly TimeoutSession _TimeoutSession;

    #endregion

    #region Constructor

    public TimeoutManager()
    {
        _TimeoutSession = new TimeoutSession();
    }

    #endregion

    #region Instance Members

    public bool TimedOut()
    {
        lock (_TimeoutSession)
        {
            return !_TimeoutSession.IsRunning();
        }
    }

    public void Dispose()
    {
        _TimeoutSession.Stop();
    }

    #region Stop

    /// <summary>
    ///     Stop
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public Milliseconds Stop()
    {
        lock (_TimeoutSession)
        {
            return _TimeoutSession.Stop();
        }
    }

    #endregion

    #endregion

    #region Start

    /// <summary>
    ///     Starts a new Timeout Session and invokes the timeoutHandler if the session times out
    /// </summary>
    /// <param name="timeout"></param>
    /// <param name="timeoutHandler"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void Start(Milliseconds timeout, Action timeoutHandler)
    {
        lock (_TimeoutSession)
        {
            if (_TimeoutSession.IsRunning())
            {
                throw new
                    InvalidOperationException($"The {nameof(TimeoutManager)} could not complete the {nameof(Start)} method because the {nameof(TimeoutManager)} is currently running");
            }

            _TimeoutSession.Start(timeout, timeoutHandler);
        }
    }

    /// <summary>
    ///     Starts a new Timeout Session
    /// </summary>
    /// <param name="timeout"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void Start(Milliseconds timeout)
    {
        lock (_TimeoutSession)
        {
            if (_TimeoutSession.IsRunning())
            {
                throw new
                    InvalidOperationException($"The {nameof(TimeoutManager)} could not complete the {nameof(Start)} method because the {nameof(TimeoutManager)} is currently running");
            }

            _TimeoutSession.Start(timeout);
        }
    }

    /// <summary>
    ///     Starts a new Timeout Session
    /// </summary>
    /// <param name="timeout"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void Start(Seconds timeout)
    {
        Start((Milliseconds) timeout);
    }

    /// <summary>
    ///     Starts a new Timeout Session
    /// </summary>
    /// <param name="timeout"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void Start(TimeSpan timeout)
    {
        Start((Milliseconds) timeout);
    }

    #endregion
}