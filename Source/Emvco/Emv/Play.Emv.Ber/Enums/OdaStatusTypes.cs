using Play.Core;
using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.Enums;

public sealed record OdaStatusTypes : EnumObject<byte>
{
    #region Static Metadata

    public static readonly OdaStatusTypes Empty = new();
    public static readonly OdaStatusTypes Cda = new(0b10000000);
    public static readonly OdaStatusTypes NotAvailable = new(0);
    private static readonly Dictionary<byte, OdaStatusTypes> _ValueObjectMap = new() {{Cda, Cda}, {NotAvailable, NotAvailable}};

    #endregion

    #region Constructor

    public OdaStatusTypes() : base()
    { }

    private OdaStatusTypes(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override OdaStatusTypes[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(byte value, out EnumObject<byte>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out OdaStatusTypes? enumResult))
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
    public static OdaStatusTypes Get(byte value)
    {
        const byte bitMask = 0b00111111;

        if (!_ValueObjectMap.ContainsKey(value.GetMaskedValue(bitMask)))
        {
            throw new DataElementParsingException(new ArgumentOutOfRangeException(nameof(value),
                $"No {nameof(OdaStatusTypes)} could be retrieved because the argument provided does not match a definition value"));
        }

        return _ValueObjectMap[value.GetMaskedValue(bitMask)];
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