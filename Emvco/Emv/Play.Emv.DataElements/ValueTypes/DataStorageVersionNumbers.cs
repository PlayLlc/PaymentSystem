using Play.Core;
using Play.Core.Extensions;

namespace Play.Emv.DataElements;

public record DataStorageVersionNumbers : EnumObject<byte>
{
    #region Static Metadata

    public static readonly DataStorageVersionNumbers NotSupported = new(0);
    public static readonly DataStorageVersionNumbers Version1 = new(0b1);
    public static readonly DataStorageVersionNumbers Version2 = new(0b10);

    private static readonly Dictionary<byte, DataStorageVersionNumbers> _ValueMap = new()
    {
        {NotSupported, NotSupported}, {Version1, Version1}, {Version2, Version2}
    };

    #endregion

    #region Constructor

    private DataStorageVersionNumbers(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public static bool IsValid(byte value) => _ValueMap.ContainsKey(value);

    public static DataStorageVersionNumbers Get(byte value)
    {
        const byte bitMask = 0b00111111;

        if (!_ValueMap.ContainsKey(value.GetMaskedValue(bitMask)))
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                                                  $"No {nameof(DataStorageVersionNumbers)} could be retrieved because the argument provided does not match a definition value");
        }

        return _ValueMap[value.GetMaskedValue(bitMask)];
    }

    #endregion

    #region Equality

    public bool Equals(DataStorageVersionNumbers x, DataStorageVersionNumbers y) => x.Equals(y);

    #endregion

    #region Operator Overrides

    public static bool operator ==(DataStorageVersionNumbers left, byte right) => left._Value == right;
    public static bool operator ==(byte left, DataStorageVersionNumbers right) => left == right._Value;
    public static explicit operator byte(DataStorageVersionNumbers value) => value._Value;
    public static explicit operator short(DataStorageVersionNumbers value) => value._Value;
    public static explicit operator ushort(DataStorageVersionNumbers value) => value._Value;
    public static explicit operator int(DataStorageVersionNumbers value) => value._Value;
    public static explicit operator uint(DataStorageVersionNumbers value) => value._Value;
    public static explicit operator long(DataStorageVersionNumbers value) => value._Value;
    public static explicit operator ulong(DataStorageVersionNumbers value) => value._Value;
    public static bool operator !=(DataStorageVersionNumbers left, byte right) => !(left == right);
    public static bool operator !=(byte left, DataStorageVersionNumbers right) => !(left == right);

    #endregion
}