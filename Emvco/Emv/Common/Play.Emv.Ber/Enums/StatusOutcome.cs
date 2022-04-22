using System.Collections.Immutable;

using Play.Core;

namespace Play.Emv.Ber.Enums;

public record StatusOutcome : EnumObject<byte>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, StatusOutcome> _ValueObjectMap;

    /// <summary>
    ///     The kernel is satisfied that the transaction is acceptable with the selected contactless card application
    ///     and wants the transaction to be approved.This is the expected Outcome for a successful offline transaction.
    ///     This might also occur following reactivation of a kernel after an online response.
    /// </summary>
    /// <remarks>
    ///     A Final Outcome. Will be returned to the terminal
    /// </remarks>
    /// <value>Decimal: 16; HexadecimalCodec: 0x10</value>
    public static readonly StatusOutcome Approved;

    /// <summary>
    ///     The kernel has found that the transaction is not  acceptable with the selected contactless card application
    ///     and wants the transaction to be declined.This might also occur following reactivation of a kernel after an
    ///     online response
    /// </summary>
    /// <remarks>
    ///     A Final Outcome. Will be returned to the terminal
    /// </remarks>
    /// <value>Decimal: 32; HexadecimalCodec: 0x20</value>
    public static readonly StatusOutcome Declined;

    /// <summary>
    ///     Any of the following
    ///     • The kernel has completed processing and requires no further action.
    ///     • The kernel wished to restart after the card has been removed.It is one way a kernel may handle a mobile
    ///     device that requires a confirmation code to be entered.
    ///     • The kernel experienced an application error, such as missing data, that will not resolve if the transaction
    ///     is attempted again with the same selected contactless card application.
    ///     • Entry Point was unable to identify a contactless card application that could complete the transaction with
    ///     the current card and wants the POS System to direct the cardholder to present another card.
    /// </summary>
    /// <remarks>
    ///     A Final Outcome. Will be returned to the terminal
    /// </remarks>
    /// <value>Decimal: 64; HexadecimalCodec: 0x40</value>
    public static readonly StatusOutcome EndApplication;

    /// The status has not yet been set
    /// </summary>
    /// <value>Decimal: 0; HexadecimalCodec: 0x0</value>
    public static readonly StatusOutcome NotAvailable;

    /// <summary>
    ///     The transaction requires an online authorization to determine the approved or declined status. If the kernel
    ///     wishes to be restarted when the response has been received(e.g.to receive issuer update data), then this is
    ///     indicated in the parameters.
    /// </summary>
    /// <remarks>
    ///     A Final Outcome. Will be returned to the terminal
    /// </remarks>
    /// <value>Decimal: 48; HexadecimalCodec: 0x30</value>
    public static readonly StatusOutcome OnlineRequest;

    /// <summary>
    ///     The kernel has determined that the selected Combination is unsuitable and the next Combination (if any)
    ///     should be tried.
    /// </summary>
    /// <remarks>
    ///     An Outcome. MainProcess will initiate processing again at Entry Point Start C
    /// </remarks>
    /// <value>Decimal: 80; HexadecimalCodec: 0x50</value>
    public static readonly StatusOutcome SelectNext;

    /// <summary>
    ///     The kernel wishes that a card be presented  again; this may be a result of an error, such as  tearing,
    ///     that could resolve if the transaction is attempted again.It is one way a kernel may handle a mobile
    ///     device that requires a confirmation code to be entered.
    /// </summary>
    /// <remarks>
    ///     An Outcome. MainProcess will initiate processing again at Entry Point Start B
    /// </remarks>
    /// <value>Decimal: 112; HexadecimalCodec: 0x70</value>
    public static readonly StatusOutcome TryAgain;

    /// <summary>
    ///     The kernel, Entry Point or Issuer is unable to complete the transaction with the selected contactless card
    ///     application, but knows from the configuration data that another interface (e.g.contact or magnetic-stripe)
    ///     is available.The kernel could indicate a preference for the alternate interface
    /// </summary>
    /// <remarks>
    ///     A Final Outcome. Will be returned to the terminal
    /// </remarks>
    /// <value>Decimal: 96; HexadecimalCodec: 0x60</value>
    public static readonly StatusOutcome TryAnotherInterface;

    #endregion

    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    static StatusOutcome()
    {
        const byte notAvailable = 0;
        const byte approved = 16;
        const byte declined = 32;
        const byte onlineRequest = 48;
        const byte endApplication = 64;
        const byte selectNext = 80;
        const byte tryAnotherInterface = 96;
        const byte tryAgain = 112;

        NotAvailable = new StatusOutcome(notAvailable);
        Approved = new StatusOutcome(approved);
        Declined = new StatusOutcome(declined);
        EndApplication = new StatusOutcome(endApplication);
        OnlineRequest = new StatusOutcome(onlineRequest);
        SelectNext = new StatusOutcome(selectNext);
        TryAgain = new StatusOutcome(tryAgain);
        TryAnotherInterface = new StatusOutcome(tryAnotherInterface);
        _ValueObjectMap = new Dictionary<byte, StatusOutcome>
        {
            {notAvailable, NotAvailable},
            {approved, Approved},
            {declined, Declined},
            {endApplication, EndApplication},
            {onlineRequest, OnlineRequest},
            {selectNext, SelectNext},
            {tryAgain, TryAgain},
            {tryAnotherInterface, TryAnotherInterface}
        }.ToImmutableSortedDictionary();
    }

    private StatusOutcome(byte value) : base(value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public override StatusOutcome[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(byte value, out EnumObject<byte>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out StatusOutcome? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    // TODO:
    // public static readonly StatusOutcome RequestOnlinePin;
    public static StatusOutcome[] GetAll() => _ValueObjectMap.Values.ToArray();

    public static StatusOutcome Get(byte value)
    {
        if (!_ValueObjectMap.TryGetValue(value, out StatusOutcome result))
            throw new ArgumentOutOfRangeException($"The argument {nameof(value)} with a value of {value} is not a valid value for {nameof(StatusOutcome)}");

        return result;
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(StatusOutcome left, byte right) => left._Value == right;
    public static bool operator ==(byte left, StatusOutcome right) => left == right._Value;
    public static explicit operator byte(StatusOutcome value) => value._Value;
    public static explicit operator short(StatusOutcome value) => value._Value;
    public static explicit operator ushort(StatusOutcome value) => value._Value;
    public static explicit operator int(StatusOutcome value) => value._Value;
    public static explicit operator uint(StatusOutcome value) => value._Value;
    public static explicit operator long(StatusOutcome value) => value._Value;
    public static explicit operator ulong(StatusOutcome value) => value._Value;
    public static bool operator !=(StatusOutcome left, byte right) => !(left == right);
    public static bool operator !=(byte left, StatusOutcome right) => !(left == right);

    #endregion
}