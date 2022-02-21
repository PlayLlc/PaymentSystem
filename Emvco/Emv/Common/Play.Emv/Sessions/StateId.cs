using System;

using Play.Emv.Identifiers;

namespace Play.Emv.Sessions;

public readonly record struct StateId
{
    #region Instance Values

    private readonly Identity _Identity;

    #endregion

    #region Constructor

    public StateId(ReadOnlySpan<char> value)
    {
        _Identity = new Identity(value);
    }

    #endregion
}