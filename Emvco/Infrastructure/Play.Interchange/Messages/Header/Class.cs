using Play.Core;
using Play.Core.Extensions;

namespace Play.Interchange.Messages.Header;

/// <summary>
///     Position two of the MTI specifies the overall purpose of the message.
/// </summary>
public sealed record Class : EnumObject<byte>
{
    #region Static Metadata

    /// <summary>
    ///     Determine if funds are available, get an approval but do not post to account for reconciliation. Dual message
    ///     system (DMS), awaits file exchange for posting to the account.
    /// </summary>
    /// <remarks>X1XX</remarks>
    public static readonly Class Authorization = new(1);

    /// <summary>
    ///     Determine if funds are available, get an approval and post directly to the account. Single message system (SMS), no
    ///     file exchange after this.
    /// </summary>
    /// <remarks>X2XX</remarks>
    public static readonly Class Financial = new(2);

    /// <summary>
    ///     Used for hot-card, TMS and other exchanges
    /// </summary>
    /// <remarks>X3XX</remarks>
    public static readonly Class FileActions = new(3);

    /// <summary>
    ///     Reversal (x4x0 or x4x1): Reverses the action of a previous authorization. Chargeback(x4x2 or x4x3) : Charges back a
    ///     previously cleared financial message.
    /// </summary>
    /// <remarks>X4XX</remarks>
    public static readonly Class Reversal = new(4);

    /// <summary>
    ///     Transmits settlement information message.
    /// </summary>
    /// <remarks>X5XX</remarks>
    public static readonly Class Reconciliation = new(5);

    /// <summary>
    ///     Transmits administrative advice. Often used for failure messages (e.g., message reject or failure to apply).
    /// </summary>
    /// <remarks>X6XX</remarks>
    public static readonly Class Administration = new(6);

    /// <remarks>X7XX</remarks>
    public static readonly Class Fee = new(7);

    /// <summary>
    ///     Used for secure key exchange, logon, echo test and other network functions.
    /// </summary>
    /// <remarks>X8XX</remarks>
    public static readonly Class Management = new(8);

    private static readonly Dictionary<byte, Class> _ValueMap = new()
    {
        {Authorization, Authorization},
        {Financial, Financial},
        {FileActions, FileActions},
        {Reversal, Reversal},
        {Reconciliation, Reconciliation},
        {Administration, Administration},
        {Fee, Fee},
        {Management, Management}
    };

    #endregion

    #region Constructor

    private Class(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public static bool IsValid(byte value) => _ValueMap.ContainsKey(value);

    public static Class Get(byte value)
    {
        const byte bitMask = 0b00111111;

        if (!_ValueMap.ContainsKey(value.GetMaskedValue(bitMask)))
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                $"No {nameof(Class)} could be retrieved because the argument provided does not match a definition value");
        }

        return _ValueMap[value.GetMaskedValue(bitMask)];
    }

    #endregion

    #region Equality

    public bool Equals(Class x, Class y) => x.Equals(y);

    #endregion

    #region Operator Overrides

    public static bool operator ==(Class left, byte right) => left._Value == right;
    public static bool operator ==(byte left, Class right) => left == right._Value;
    public static implicit operator ushort(Class value) => value._Value;
    public static bool operator !=(Class left, byte right) => !(left == right);
    public static bool operator !=(byte left, Class right) => !(left == right);

    #endregion
}