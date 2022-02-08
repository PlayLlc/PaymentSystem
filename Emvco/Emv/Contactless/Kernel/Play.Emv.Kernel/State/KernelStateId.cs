using System;

using Play.Emv.Identifiers;

namespace Play.Emv.Kernel.State;

public readonly record struct KernelStateId
{
    #region Instance Values

    private readonly Identity _Identity;

    #endregion

    #region Constructor

    public KernelStateId(ReadOnlySpan<char> value)
    {
        _Identity = new Identity(value);
    }

    #endregion
}