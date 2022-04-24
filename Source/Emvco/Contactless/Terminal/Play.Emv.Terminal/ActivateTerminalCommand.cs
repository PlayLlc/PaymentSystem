using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;

namespace Play.Emv.Terminal;

public class ActivateTerminalCommand
{
    #region Instance Values

    private readonly AmountAuthorizedNumeric _AmountAuthorizedNumeric;
    private readonly AmountOtherNumeric _AmountOtherNumeric;
    private readonly TransactionType _TransactionType;
    private readonly TerminalIdentification _TerminalIdentification;
    private readonly AcquirerIdentifier _AcquirerIdentifier;
    private readonly MerchantIdentifier _MerchantIdentifier;

    #endregion

    #region Constructor

    public ActivateTerminalCommand(
        string terminalIdentification, ulong amountAuthorized, ulong otherAmount, byte transactionType, ulong acquirerIdentifier,
        string merchantIdentifier)
    {
        _AmountAuthorizedNumeric = new AmountAuthorizedNumeric(amountAuthorized);
        _AmountOtherNumeric = new AmountOtherNumeric(otherAmount);
        _TransactionType = new TransactionType(transactionType);
        _TerminalIdentification = new TerminalIdentification(terminalIdentification);
        _AcquirerIdentifier = new AcquirerIdentifier(acquirerIdentifier);
        _MerchantIdentifier = new MerchantIdentifier(merchantIdentifier);
    }

    #endregion

    #region Instance Members

    public MerchantIdentifier GetMerchantIdentifier() => _MerchantIdentifier;
    public TerminalIdentification GetTerminalIdentification() => _TerminalIdentification;
    public AcquirerIdentifier GetAcquirerIdentifier() => _AcquirerIdentifier;
    public AmountAuthorizedNumeric GetAmountAuthorizedNumeric() => _AmountAuthorizedNumeric;
    public AmountOtherNumeric GetAmountOtherNumeric() => _AmountOtherNumeric;
    public TransactionType GetTransactionType() => _TransactionType;

    #endregion
}