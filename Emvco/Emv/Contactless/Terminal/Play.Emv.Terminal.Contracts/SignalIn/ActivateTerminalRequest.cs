using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Messaging;
using Play.Messaging;

namespace Play.Emv.Terminal.Contracts.SignalIn;

public record ActivateTerminalRequest : RequestSignal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(ActivateTerminalRequest));
    public static readonly ChannelTypeId ChannelTypeId = ChannelType.Terminal;

    #endregion

    #region Instance Values

    private readonly MessageTypeIndicator _MessageTypeIndicator;
    private readonly AccountType _AccountType;
    private readonly AmountAuthorizedNumeric _AmountAuthorizedNumeric;
    private readonly AmountOtherNumeric _AmountOtherNumeric;
    private readonly TransactionType _TransactionType;
    private readonly TerminalIdentification _TerminalIdentification;
    private readonly AcquirerIdentifier _AcquirerIdentifier;
    private readonly MerchantIdentifier _MerchantIdentifier;
    private readonly PosEntryMode _PosEntryMode;

    #endregion

    #region Constructor

    public ActivateTerminalRequest(
        MessageTypeIndicator messageTypeIndicator, PosEntryMode posEntryMode, string terminalIdentification, byte accountType,
        ulong amountAuthorized, ulong otherAmount, byte transactionType, ulong acquirerIdentifier, string merchantIdentifier) : base(
        MessageTypeId, ChannelTypeId)
    {
        _PosEntryMode = posEntryMode;
        _MessageTypeIndicator = messageTypeIndicator;
        _AccountType = new AccountType(accountType);
        _AmountAuthorizedNumeric = new AmountAuthorizedNumeric(amountAuthorized);
        _AmountOtherNumeric = new AmountOtherNumeric(otherAmount);
        _TransactionType = new TransactionType(transactionType);
        _TerminalIdentification = new TerminalIdentification(terminalIdentification);
        _AcquirerIdentifier = new AcquirerIdentifier(acquirerIdentifier);
        _MerchantIdentifier = new MerchantIdentifier(merchantIdentifier);
    }

    #endregion

    #region Instance Members

    public MessageTypeIndicator GetMessageTypeIndicator() => _MessageTypeIndicator;
    public PosEntryMode GetPosEntryMode() => _PosEntryMode;
    public AccountType GetAccountType() => _AccountType;
    public MerchantIdentifier GetMerchantIdentifier() => _MerchantIdentifier;
    public TerminalIdentification GetTerminalIdentification() => _TerminalIdentification;
    public AcquirerIdentifier GetAcquirerIdentifier() => _AcquirerIdentifier;
    public AmountAuthorizedNumeric GetAmountAuthorizedNumeric() => _AmountAuthorizedNumeric;
    public AmountOtherNumeric GetAmountOtherNumeric() => _AmountOtherNumeric;
    public TransactionType GetTransactionType() => _TransactionType;

    #endregion
}