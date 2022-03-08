using System;

using Play.Core.Extensions;

namespace Play.Emv.DataElements.Emv;

public readonly record struct DataStorageVersionNumber
{
    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    public DataStorageVersionNumber(byte value)
    {
        if (value.AreBitsSet(0b00111111))
            throw new ArgumentOutOfRangeException(nameof(value));

        _Value = value;
    }

    #endregion

    #region Operator Overrides

    public static explicit operator byte(DataStorageVersionNumber value) => value._Value;

    #endregion
}