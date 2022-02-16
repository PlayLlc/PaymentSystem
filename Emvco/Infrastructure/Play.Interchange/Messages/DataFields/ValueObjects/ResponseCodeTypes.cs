using Play.Core;

namespace Play.Interchange.Messages.DataFields.ValueObjects;

public record ResponseCodeTypes : EnumObject<ushort>
{
    #region Static Metadata

    public static readonly ResponseCodeTypes Approved134InvalidExpiryDate = new(0);
    public static readonly ResponseCodeTypes PartialApproval135InvalidBin = new(1);
    public static readonly ResponseCodeTypes DoNotHonor136InvalidCurrency = new(100);
    public static readonly ResponseCodeTypes ExpiredCard137InvalidBatchNumber = new(101);
    public static readonly ResponseCodeTypes PinRetriesExceeded138InsufficientFundsOverCreditLimit1 = new(106);
    public static readonly ResponseCodeTypes ReferToCardIssuer139TransactionNotPermittedToIssuerCardholder1 = new(107);
    public static readonly ResponseCodeTypes ReferCardIssuerSpecialConditions = new(108);
    public static readonly ResponseCodeTypes ExceedsWithdrawalCountLimit1 = new(140);
    public static readonly ResponseCodeTypes InvalidMerchant190NonDcTransactionNotAllowed = new(109);
    public static readonly ResponseCodeTypes InvalidAmount301NotSupported = new(110);
    public static readonly ResponseCodeTypes InvalidCardNumber306NotSuccessful = new(111);
    public static readonly ResponseCodeTypes InvalidPin400SuccessfulReversal = new(117);
    public static readonly ResponseCodeTypes NoCardRecord500ReconciliationInBalance = new(118);
    public static readonly ResponseCodeTypes TransactionNotPermittedToTerminal = new(120);
    public static readonly ResponseCodeTypes ReconciliationOutOfBalance = new(501);
    public static readonly ResponseCodeTypes ExceedsWithdrawalFrequencyLimit = new(123);
    public static readonly ResponseCodeTypes InvalidTransaction = new(902);
    public static readonly ResponseCodeTypes CardNotActive904FormatError = new(125);
    public static readonly ResponseCodeTypes InvalidTerminal909Malfunction = new(130);
    public static readonly ResponseCodeTypes SequenceNumberError916MacIncorrect = new(131);
    public static readonly ResponseCodeTypes MustSettleBatch992InconsistentWithMessageSpecification = new(132);
    public static readonly ResponseCodeTypes NoTransactions = new(133);

    #endregion

    #region Constructor

    public ResponseCodeTypes(ushort value) : base(value)
    { }

    #endregion
}