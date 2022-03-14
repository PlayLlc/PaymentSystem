using Play.Core;
using Play.Core.Extensions;

namespace Play.Interchange.Messages.Header;

/// <summary>
///     Position three of the MTI specifies the message function which defines how the message should flow within the
///     system. Requests are end-to-end messages (e.g., from acquirer to issuer and back with time-outs and automatic
///     reversals in place), while advices are point-to-point messages (e.g., from terminal to acquirer, from acquirer to
///     network, from network to issuer, with transmission guaranteed over each link, but not necessarily immediately).
/// </summary>
public sealed record Function : EnumObject<byte>
{
    #region Static Metadata

    /// <summary>
    ///     Request from acquirer to issuer to carry out an action; issuer may accept or reject
    /// </summary>
    /// <remarks>X0XX</remarks>
    public static readonly Function Request = new(0);

    /// <summary>
    ///     Issuer response to a request
    /// </summary>
    /// <remarks>X1XX</remarks>
    public static readonly Function RequestResponse = new(1);

    /// <summary>
    ///     Advice that an action has taken place; receiver can only accept, not reject
    /// </summary>
    /// <remarks>X2XX</remarks>
    public static readonly Function Advice = new(2);

    /// <summary>
    ///     Response to an advice
    /// </summary>
    /// <remarks>X3XX</remarks>
    public static readonly Function AdviceResponse = new(3);

    /// <summary>
    ///     Notification that an event has taken place; receiver can only accept, not reject
    /// </summary>
    /// <remarks>X4XX</remarks>
    public static readonly Function Notification = new(4);

    /// <summary>
    ///     Response to a notification
    /// </summary>
    /// <remarks>X5XX</remarks>
    public static readonly Function NotificationAcknowledgement = new(5);

    /// <summary>
    ///     ISO 8583:2003
    /// </summary>
    /// <remarks>X6XX</remarks>
    public static readonly Function Instruction = new(6);

    /// <summary>
    ///     ISO 8583:2003
    /// </summary>
    /// <remarks>X7XX</remarks>
    public static readonly Function InstructionAcknowledgement = new(7);

    private static readonly Dictionary<byte, Function> _ValueMap = new()
    {
        {Request, Request},
        {RequestResponse, RequestResponse},
        {Advice, Advice},
        {AdviceResponse, AdviceResponse},
        {Notification, Notification},
        {NotificationAcknowledgement, NotificationAcknowledgement},
        {Instruction, Instruction},
        {InstructionAcknowledgement, InstructionAcknowledgement}
    };

    #endregion

    #region Constructor

    private Function(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public static bool IsValid(byte value) => _ValueMap.ContainsKey(value);

    public static Function Get(byte value)
    {
        const byte bitMask = 0b00111111;

        if (!_ValueMap.ContainsKey(value.GetMaskedValue(bitMask)))
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                                                  $"No {nameof(Function)} could be retrieved because the argument provided does not match a definition value");
        }

        return _ValueMap[value.GetMaskedValue(bitMask)];
    }

    #endregion

    #region Equality

    public bool Equals(Function x, Function y) => x.Equals(y);

    #endregion

    #region Operator Overrides

    public static bool operator ==(Function left, byte right) => left._Value == right;
    public static bool operator ==(byte left, Function right) => left == right._Value;
    public static implicit operator ushort(Function value) => value._Value;
    public static bool operator !=(Function left, byte right) => !(left == right);
    public static bool operator !=(byte left, Function right) => !(left == right);

    #endregion
}