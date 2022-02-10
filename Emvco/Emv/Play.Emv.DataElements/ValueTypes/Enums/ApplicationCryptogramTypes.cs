using Play.Core;
using Play.Core.Extensions;

namespace Play.Emv.DataElements;

public record ApplicationCryptogramTypes : EnumObject<byte>
{
    #region Static Metadata

    public static readonly ApplicationCryptogramTypes Aac = new(0);
    public static readonly ApplicationCryptogramTypes Tc = new(0b01000000);
    public static readonly ApplicationCryptogramTypes Arqc = new(0b10000000);
    private static readonly Dictionary<byte, ApplicationCryptogramTypes> _ValueMap = new() {{Aac, Aac}, {Tc, Tc}, {Arqc, Arqc}};

    #endregion

    #region Constructor

    private ApplicationCryptogramTypes(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public static bool IsValid(byte value) => _ValueMap.ContainsKey(value);

    public static ApplicationCryptogramTypes Get(byte value)
    {
        const byte bitMask = 0b00111111;

        if (!_ValueMap.ContainsKey(value.GetMaskedValue(bitMask)))
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                                                  $"No {nameof(ApplicationCryptogramTypes)} could be retrieved because the argument provided does not match a definition value");
        }

        return _ValueMap[value.GetMaskedValue(bitMask)];
    }

    #endregion

    #region Equality

    public bool Equals(ApplicationCryptogramTypes x, ApplicationCryptogramTypes y) => x.Equals(y);

    #endregion

    #region Operator Overrides

    public static bool operator ==(ApplicationCryptogramTypes left, byte right) => left._Value == right;
    public static bool operator ==(byte left, ApplicationCryptogramTypes right) => left == right._Value;
    public static explicit operator byte(ApplicationCryptogramTypes value) => value._Value;
    public static explicit operator short(ApplicationCryptogramTypes value) => value._Value;
    public static explicit operator ushort(ApplicationCryptogramTypes value) => value._Value;
    public static explicit operator int(ApplicationCryptogramTypes value) => value._Value;
    public static explicit operator uint(ApplicationCryptogramTypes value) => value._Value;
    public static explicit operator long(ApplicationCryptogramTypes value) => value._Value;
    public static explicit operator ulong(ApplicationCryptogramTypes value) => value._Value;
    public static bool operator !=(ApplicationCryptogramTypes left, byte right) => !(left == right);
    public static bool operator !=(byte left, ApplicationCryptogramTypes right) => !(left == right);

    #endregion
}