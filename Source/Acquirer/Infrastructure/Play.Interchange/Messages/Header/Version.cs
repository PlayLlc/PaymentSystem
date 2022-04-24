using Play.Core;
using Play.Core.Extensions;

namespace Play.Interchange.Messages.Header;

/// <summary>
///     The first digit of the MTI indicates the ISO 8583 version in which the message is encoded.
/// </summary>
public sealed record Version : EnumObject<byte>
{
    #region Static Metadata

    /// <summary>ISO 8583:1987</summary>
    /// <remarks>X1XXX</remarks>
    public static readonly Version _1987 = new(0);

    /// <summary>ISO 8583:1993</summary>
    /// <remarks>X1XXX</remarks>
    public static readonly Version _1993 = new(1);

    /// <summary>ISO 8583:2003</summary>
    /// <remarks>X2XXX</remarks>
    public static readonly Version _2003 = new(2);

    /// <summary>National use</summary>
    /// <remarks>X8XXX</remarks>
    public static readonly Version NationalUse = new(8);

    /// <summary>Private use</summary>
    /// <remarks>X9XXX</remarks>
    public static readonly Version PrivateUse = new(9);

    private static readonly Dictionary<byte, Version> _ValueObjectMap = new()
    {
        {_1987, _1987}, {_1993, _1993}, {_2003, _2003}, {NationalUse, NationalUse}, {PrivateUse, PrivateUse}
    };

    #endregion

    #region Constructor

    private Version(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override Version[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(byte value, out EnumObject<byte>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out Version? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    public static bool IsValid(byte value) => _ValueObjectMap.ContainsKey(value);

    public static Version Get(byte value)
    {
        const byte bitMask = 0b00111111;

        if (!_ValueObjectMap.ContainsKey(value.GetMaskedValue(bitMask)))
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                $"No {nameof(Version)} could be retrieved because the argument provided does not match a definition value");
        }

        return _ValueObjectMap[value.GetMaskedValue(bitMask)];
    }

    #endregion

    #region Equality

    public bool Equals(Version x, Version y) => x.Equals(y);

    #endregion

    #region Operator Overrides

    public static bool operator ==(Version left, byte right) => left._Value == right;
    public static bool operator ==(byte left, Version right) => left == right._Value;
    public static implicit operator ushort(Version value) => value._Value;
    public static bool operator !=(Version left, byte right) => !(left == right);
    public static bool operator !=(byte left, Version right) => !(left == right);

    #endregion
}