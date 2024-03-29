﻿using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.ValueTypes;

public readonly record struct DataStorageVersionNumber
{
    #region Instance Values

    private readonly byte _Value;

    #endregion

    public DataStorageVersionNumber(byte value)
    {
        if (value.AreBitsSet(0b11111100))
            throw new TerminalDataException(new ArgumentOutOfRangeException(nameof(value)), $"The {nameof(DataStorageVersionNumber)} had invalid bits set. Bits 1 through 6 must not be set");

        _Value = value;
    }

    #region Operator Overrides

    public static implicit operator byte(DataStorageVersionNumber value) => value._Value;

    #endregion
}