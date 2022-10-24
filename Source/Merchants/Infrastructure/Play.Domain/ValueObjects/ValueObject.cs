﻿namespace Play.Domain.ValueObjects;

public abstract record ValueObject<_T>
{
    #region Instance Values

    public _T Value { get; }

    #endregion

    #region Constructor

    protected ValueObject(_T value)
    {
        Value = value;
    }

    #endregion
}