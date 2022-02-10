using Play.Core;
using Play.Core.Extensions;

namespace Play.Emv.DataElements;

public record DataStorageVersionNumberTypes : EnumObject<byte>
{
    #region Static Metadata

    public static readonly DataStorageVersionNumberTypes NotSupported = new(0);
    public static readonly DataStorageVersionNumberTypes Version1 = new(0b1);
    public static readonly DataStorageVersionNumberTypes Version2 = new(0b10);

    private static readonly Dictionary<byte, DataStorageVersionNumberTypes> _ValueMap = new()
    {
        {NotSupported, NotSupported}, {Version1, Version1}, {Version2, Version2}
    };

    #endregion

    #region Constructor

    private DataStorageVersionNumberTypes(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public static bool IsValid(byte value) => _ValueMap.ContainsKey(value);

    public static DataStorageVersionNumberTypes Get(byte value)
    {
        const byte bitMask = 0b00111111;

        if (!_ValueMap.ContainsKey(value.GetMaskedValue(bitMask)))
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                                                  $"No {nameof(DataStorageVersionNumberTypes)} could be retrieved because the argument provided does not match a definition value");
        }

        return _ValueMap[value.GetMaskedValue(bitMask)];
    }

    #endregion

    #region Equality

    public bool Equals(DataStorageVersionNumberTypes x, DataStorageVersionNumberTypes y) => x.Equals(y);

    #endregion

    #region Operator Overrides

    public static bool operator ==(DataStorageVersionNumberTypes left, byte right) => left._Value == right;
    public static bool operator ==(byte left, DataStorageVersionNumberTypes right) => left == right._Value;
    public static explicit operator byte(DataStorageVersionNumberTypes value) => value._Value;
    public static explicit operator short(DataStorageVersionNumberTypes value) => value._Value;
    public static explicit operator ushort(DataStorageVersionNumberTypes value) => value._Value;
    public static explicit operator int(DataStorageVersionNumberTypes value) => value._Value;
    public static explicit operator uint(DataStorageVersionNumberTypes value) => value._Value;
    public static explicit operator long(DataStorageVersionNumberTypes value) => value._Value;
    public static explicit operator ulong(DataStorageVersionNumberTypes value) => value._Value;
    public static bool operator !=(DataStorageVersionNumberTypes left, byte right) => !(left == right);
    public static bool operator !=(byte left, DataStorageVersionNumberTypes right) => !(left == right);

    #endregion
}