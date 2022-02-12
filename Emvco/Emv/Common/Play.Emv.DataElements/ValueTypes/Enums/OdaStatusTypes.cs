using Play.Core;
using Play.Core.Extensions;

namespace Play.Emv.DataElements;

public sealed record OdaStatusTypes : EnumObject<byte>
{
    #region Static Metadata

    public static readonly OdaStatusTypes Cda = new(0b10000000);
    public static readonly OdaStatusTypes NotAvailable = new(0);
    private static readonly Dictionary<byte, OdaStatusTypes> _ValueMap = new() {{Cda, Cda}};

    #endregion

    #region Constructor

    private OdaStatusTypes(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public static bool IsValid(byte value) => _ValueMap.ContainsKey(value);

    public static OdaStatusTypes Get(byte value)
    {
        const byte bitMask = 0b00111111;

        if (!_ValueMap.ContainsKey(value.GetMaskedValue(bitMask)))
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                                                  $"No {nameof(OdaStatusTypes)} could be retrieved because the argument provided does not match a definition value");
        }

        return _ValueMap[value.GetMaskedValue(bitMask)];
    }

    #endregion

    #region Equality

    public bool Equals(OdaStatusTypes x, OdaStatusTypes y) => x.Equals(y);

    #endregion

    #region Operator Overrides

    public static bool operator ==(OdaStatusTypes left, byte right) => left._Value == right;
    public static bool operator ==(byte left, OdaStatusTypes right) => left == right._Value;
    public static explicit operator byte(OdaStatusTypes value) => value._Value;
    public static explicit operator short(OdaStatusTypes value) => value._Value;
    public static explicit operator ushort(OdaStatusTypes value) => value._Value;
    public static explicit operator int(OdaStatusTypes value) => value._Value;
    public static explicit operator uint(OdaStatusTypes value) => value._Value;
    public static explicit operator long(OdaStatusTypes value) => value._Value;
    public static explicit operator ulong(OdaStatusTypes value) => value._Value;
    public static bool operator !=(OdaStatusTypes left, byte right) => !(left == right);
    public static bool operator !=(byte left, OdaStatusTypes right) => !(left == right);

    #endregion
}