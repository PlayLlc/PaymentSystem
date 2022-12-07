using Play.Core;
using Play.Core.Extensions;

namespace Play.Emv.DataElements;

public record OdaStatus : EnumObject<byte>
{
    #region Static Metadata

    public static readonly OdaStatus Cda = new(0b10000000);
    public static readonly OdaStatus NotAvailable = new(0);
    private static readonly Dictionary<byte, OdaStatus> _ValueMap = new() {{Cda, Cda}};

    #endregion

    #region Constructor

    private OdaStatus(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public static bool IsValid(byte value) => _ValueMap.ContainsKey(value);

    public static OdaStatus Get(byte value)
    {
        const byte bitMask = 0b00111111;

        if (!_ValueMap.ContainsKey(value.GetMaskedValue(bitMask)))
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                $"No {nameof(OdaStatus)} could be retrieved because the argument provided does not match a definition value");
        }

        return _ValueMap[value.GetMaskedValue(bitMask)];
    }

    #endregion

    #region Equality

    public bool Equals(OdaStatus x, OdaStatus y) => x.Equals(y);

    #endregion

    #region Operator Overrides

    public static bool operator ==(OdaStatus left, byte right) => left._Value == right;
    public static bool operator ==(byte left, OdaStatus right) => left == right._Value;
    public static explicit operator byte(OdaStatus value) => value._Value;
    public static explicit operator short(OdaStatus value) => value._Value;
    public static explicit operator ushort(OdaStatus value) => value._Value;
    public static explicit operator int(OdaStatus value) => value._Value;
    public static explicit operator uint(OdaStatus value) => value._Value;
    public static explicit operator long(OdaStatus value) => value._Value;
    public static explicit operator ulong(OdaStatus value) => value._Value;
    public static bool operator !=(OdaStatus left, byte right) => !(left == right);
    public static bool operator !=(byte left, OdaStatus right) => !(left == right);

    #endregion
}