using Play.Core;
using Play.Core.Extensions;

namespace Play.Emv.DataElements;

public record AcType : EnumObject<byte>
{
    #region Static Metadata

    public static readonly AcType Aac = new(0);
    public static readonly AcType Tc = new(0b01000000);
    public static readonly AcType Arqc = new(0b10000000);
    private static readonly Dictionary<byte, AcType> _ValueMap = new() {{Aac, Aac}, {Tc, Tc}, {Arqc, Arqc}};

    #endregion

    #region Constructor

    private AcType(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public static bool IsValid(byte value) => _ValueMap.ContainsKey(value);

    public static AcType Get(byte value)
    {
        const byte bitMask = 0b00111111;

        if (!_ValueMap.ContainsKey(value.GetMaskedValue(bitMask)))
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                $"No {nameof(AcType)} could be retrieved because the argument provided does not match a definition value");
        }

        return _ValueMap[value.GetMaskedValue(bitMask)];
    }

    #endregion

    #region Equality

    public bool Equals(AcType x, AcType y) => x.Equals(y);

    #endregion

    #region Operator Overrides

    public static bool operator ==(AcType left, byte right) => left._Value == right;
    public static bool operator ==(byte left, AcType right) => left == right._Value;
    public static explicit operator byte(AcType value) => value._Value;
    public static explicit operator short(AcType value) => value._Value;
    public static explicit operator ushort(AcType value) => value._Value;
    public static explicit operator int(AcType value) => value._Value;
    public static explicit operator uint(AcType value) => value._Value;
    public static explicit operator long(AcType value) => value._Value;
    public static explicit operator ulong(AcType value) => value._Value;
    public static bool operator !=(AcType left, byte right) => !(left == right);
    public static bool operator !=(byte left, AcType right) => !(left == right);

    #endregion
}