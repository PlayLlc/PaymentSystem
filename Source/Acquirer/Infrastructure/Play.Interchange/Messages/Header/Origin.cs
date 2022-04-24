using Play.Core;
using Play.Core.Extensions;

namespace Play.Interchange.Messages.Header;

/// <summary>
///     Position four of the MTI defines the location of the message source within the payment chain.
/// </summary>
public sealed record Origin : EnumObject<byte>
{
    #region Static Metadata

    /// <summary>
    ///     Acquirer
    /// </summary>
    /// <remarks>X0XX</remarks>
    public static readonly Origin Acquirer = new(0);

    /// <summary>
    ///     Acquirer Repeat
    /// </summary>
    /// <remarks>X1XX</remarks>
    public static readonly Origin AcquirerRepeat = new(1);

    /// <summary>
    ///     Issuer
    /// </summary>
    /// <remarks>X2XX</remarks>
    public static readonly Origin Issuer = new(2);

    /// <summary>
    ///     Issuer Repeat
    /// </summary>
    /// <remarks>X3XX</remarks>
    public static readonly Origin IssuerRepeat = new(3);

    /// <summary>
    ///     Other
    /// </summary>
    /// <remarks>X4XX</remarks>
    public static readonly Origin Other = new(4);

    /// <summary>
    ///     Other Repeat
    /// </summary>
    /// <remarks>X5XX</remarks>
    public static readonly Origin OtherRepeat = new(5);

    private static readonly Dictionary<byte, Origin> _ValueObjectMap = new()
    {
        {Acquirer, Acquirer}, {AcquirerRepeat, AcquirerRepeat}, {Issuer, Issuer}, {IssuerRepeat, IssuerRepeat}, {Other, Other}, {OtherRepeat, OtherRepeat}
    };

    #endregion

    #region Constructor

    private Origin(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override Origin[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(byte value, out EnumObject<byte>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out Origin? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    public static bool IsValid(byte value) => _ValueObjectMap.ContainsKey(value);

    public static Origin Get(byte value)
    {
        const byte bitMask = 0b00111111;

        if (!_ValueObjectMap.ContainsKey(value.GetMaskedValue(bitMask)))
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                $"No {nameof(Origin)} could be retrieved because the argument provided does not match a definition value");
        }

        return _ValueObjectMap[value.GetMaskedValue(bitMask)];
    }

    #endregion

    #region Equality

    public bool Equals(Origin x, Origin y) => x.Equals(y);

    #endregion

    #region Operator Overrides

    public static bool operator ==(Origin left, byte right) => left._Value == right;
    public static bool operator ==(byte left, Origin right) => left == right._Value;
    public static implicit operator ushort(Origin value) => value._Value;
    public static bool operator !=(Origin left, byte right) => !(left == right);
    public static bool operator !=(byte left, Origin right) => !(left == right);

    #endregion
}