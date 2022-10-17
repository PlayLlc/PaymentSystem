using Play.Core;
using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;

namespace Play.Emv.Ber.Enums;

public sealed record DataStorageVersionNumbers : EnumObject<byte>
{
    #region Static Metadata

    public static readonly DataStorageVersionNumbers Empty = new();
    public static readonly DataStorageVersionNumbers NotSupported = new(0);
    public static readonly DataStorageVersionNumbers Version1 = new(0b100_0000);
    public static readonly DataStorageVersionNumbers Version2 = new(0b1000_0000);

    private static readonly Dictionary<byte, DataStorageVersionNumbers> _ValueObjectMap = new()
    {
        {NotSupported, NotSupported}, {Version1, Version1}, {Version2, Version2}
    };

    #endregion

    #region Constructor

    public DataStorageVersionNumbers()
    { }

    private DataStorageVersionNumbers(byte value) : base(new DataStorageVersionNumber(value))
    { }

    #endregion

    #region Instance Members

    public override DataStorageVersionNumbers[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(byte value, out EnumObject<byte>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out DataStorageVersionNumbers? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    public static bool IsValid(byte value) => _ValueObjectMap.ContainsKey(value);

    /// <summary>
    ///     Get
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="DataElementParsingException"></exception>
    public static DataStorageVersionNumbers Get(byte value)
    {
        const byte bitMask = 0b00111111;

        if (!_ValueObjectMap.ContainsKey(value.GetMaskedValue(bitMask)))
        {
            throw new DataElementParsingException(new ArgumentOutOfRangeException(nameof(value),
                $"No {nameof(DataStorageVersionNumbers)} could be retrieved because the argument provided does not match a definition value"));
        }

        return _ValueObjectMap[value.GetMaskedValue(bitMask)];
    }

    #endregion

    #region Equality

    public bool Equals(DataStorageVersionNumbers x, DataStorageVersionNumbers y) => x.Equals(y);

    #endregion

    #region Operator Overrides

    public static bool operator ==(DataStorageVersionNumbers left, byte right) => left._Value == right;
    public static bool operator ==(byte left, DataStorageVersionNumbers right) => left == right._Value;
    public static explicit operator byte(DataStorageVersionNumbers value) => value._Value;
    public static bool operator !=(DataStorageVersionNumbers left, byte right) => !(left == right);
    public static bool operator !=(byte left, DataStorageVersionNumbers right) => !(left == right);

    #endregion
}