using System.Collections.Immutable;

namespace Play.Emv.DataElements;

public readonly struct StatusOutcome
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
    /// <value>Decimal: 16; Hexadecimal: 0x10</value>
    public static readonly StatusOutcome Approved;

    /// <summary>
    ///     The kernel has found that the transaction is not  acceptable with the selected contactless card application
    ///     and wants the transaction to be declined.This might also occur following reactivation of a kernel after an
    ///     online response
    /// </summary>
    /// <remarks>
    ///     A Final Outcome. Will be returned to the terminal
    /// </remarks>
    /// <value>Decimal: 32; Hexadecimal: 0x20</value>
    public static readonly StatusOutcome Declined;

    /// <summary>
    ///     Any of the following
    ///     � The kernel has completed processing and requires no further action.
    ///     � The kernel wished to restart after the card has been removed.It is one way a kernel may handle a mobile
    ///     device that requires a confirmation code to be entered.
    ///     � The kernel experienced an application error, such as missing data, that will not resolve if the transaction
    ///     is attempted again with the same selected contactless card application.
    ///     � Entry Point was unable to identify a contactless card application that could complete the transaction with
    ///     the current card and wants the POS System to direct the cardholder to present another card.
    /// </summary>
    /// <remarks>
    ///     A Final Outcome. Will be returned to the terminal
    /// </remarks>
    /// <value>Decimal: 64; Hexadecimal: 0x40</value>
    public static readonly StatusOutcome EndApplication;

    // TODO:
    // public static readonly StatusOutcome RequestOnlinePin;

    /// <summary>
    ///     The status has not yet been set
    /// </summary>
    /// <value>Decimal: 0; Hexadecimal: 0x0</value>
    public static readonly StatusOutcome NotAvailable;

    /// <summary>
    ///     The transaction requires an online authorization to determine the approved or declined status. If the kernel
    ///     wishes to be restarted when the response has been received(e.g.to receive issuer update data), then this is
    ///     indicated in the parameters.
    /// </summary>
    /// <remarks>
    ///     A Final Outcome. Will be returned to the terminal
    /// </remarks>
    /// <value>Decimal: 48; Hexadecimal: 0x30</value>
    public static readonly StatusOutcome OnlineRequest;

    /// <summary>
    ///     The kernel has determined that the selected Combination is unsuitable and the next Combination (if any)
    ///     should be tried.
    /// </summary>
    /// <remarks>
    ///     An Outcome. MainProcess will initiate processing again at Entry Point Start C
    /// </remarks>
    /// <value>Decimal: 80; Hexadecimal: 0x50</value>
    public static readonly StatusOutcome SelectNext;

    /// <summary>
    ///     The kernel wishes that a card be presented  again; this may be a result of an error, such as  tearing,
    ///     that could resolve if the transaction is attempted again.It is one way a kernel may handle a mobile
    ///     device that requires a confirmation code to be entered.
    /// </summary>
    /// <remarks>
    ///     An Outcome. MainProcess will initiate processing again at Entry Point Start B
    /// </remarks>
    /// <value>Decimal: 112; Hexadecimal: 0x70</value>
    public static readonly StatusOutcome TryAgain;

    /// <summary>
    ///     The kernel, Entry Point or Issuer is unable to complete the transaction with the selected contactless card
    ///     application, but knows from the configuration data that another interface (e.g.contact or magnetic-stripe)
    ///     is available.The kernel could indicate a preference for the alternate interface
    /// </summary>
    /// <remarks>
    ///     A Final Outcome. Will be returned to the terminal
    /// </remarks>
    /// <value>Decimal: 96; Hexadecimal: 0x60</value>
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

    private StatusOutcome(byte value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public static StatusOutcome Get(byte value)
    {
        if (!_ValueObjectMap.TryGetValue(value, out StatusOutcome result))
        {
            throw new ArgumentOutOfRangeException(
                $"The argument {nameof(value)} with a value of {value} is not a valid value for {nameof(StatusOutcome)}");
        }

        return result;
    }

    #endregion

    #region Equality

    public override bool Equals(object? obj) => obj is StatusOutcome outcomeParameterStatus && Equals(outcomeParameterStatus);
    public bool Equals(StatusOutcome other) => _Value == other._Value;
    public bool Equals(StatusOutcome x, StatusOutcome y) => x.Equals(y);
    public bool Equals(byte other) => _Value == other;

    public override int GetHashCode()
    {
        const int hash = 673459;

        return hash + _Value.GetHashCode();
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(StatusOutcome left, StatusOutcome right) => left._Value == right._Value;
    public static bool operator ==(StatusOutcome left, byte right) => left._Value == right;
    public static bool operator ==(byte left, StatusOutcome right) => left == right._Value;
    public static explicit operator byte(StatusOutcome value) => value._Value;
    public static explicit operator short(StatusOutcome value) => value._Value;
    public static explicit operator ushort(StatusOutcome value) => value._Value;
    public static explicit operator int(StatusOutcome value) => value._Value;
    public static explicit operator uint(StatusOutcome value) => value._Value;
    public static explicit operator long(StatusOutcome value) => value._Value;
    public static explicit operator ulong(StatusOutcome value) => value._Value;
    public static bool operator !=(StatusOutcome left, StatusOutcome right) => !(left == right);
    public static bool operator !=(StatusOutcome left, byte right) => !(left == right);
    public static bool operator !=(byte left, StatusOutcome right) => !(left == right);

    #endregion
}