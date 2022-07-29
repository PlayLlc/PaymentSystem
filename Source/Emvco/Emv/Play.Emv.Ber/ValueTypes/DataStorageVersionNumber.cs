using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.ValueTypes;

public readonly record struct DataStorageVersionNumber
{
    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Static Values

    public static readonly DataStorageVersionNumber DataStorageNotSupported;
    public static readonly DataStorageVersionNumber DataStorageVersion1;
    public static readonly DataStorageVersionNumber DataStorageVersion2;

    #endregion

    #region Constructor

    static DataStorageVersionNumber()
    {
        DataStorageNotSupported = new DataStorageVersionNumber(0);
        DataStorageVersion1 = new DataStorageVersionNumber(1);
        DataStorageVersion2 = new DataStorageVersionNumber(2);
    }

    public DataStorageVersionNumber(byte value)
    {
        if (value.AreBitsSet(0b00111111))
            throw new TerminalDataException(new ArgumentOutOfRangeException(nameof(value)), $"The {nameof(DataStorageVersionNumber)} had invalid bits set. Bits 1 through 6 must not be set");

        _Value = value;
    }

    #endregion

    #region Operator Overrides

    public static implicit operator byte(DataStorageVersionNumber value) => value._Value;

    #endregion
}