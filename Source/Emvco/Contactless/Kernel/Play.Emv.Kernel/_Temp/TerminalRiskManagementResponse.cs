using Play.Emv.Ber.Enums;
using Play.Emv.Ber.ValueTypes;

namespace Play.Emv.Terminal.Contracts.Messages.Commands;

public class TerminalRiskManagementResponse
{
    #region Instance Values

    /// <summary>
    ///     The result of terminal risk management is the setting of appropriate bits in the TVR.
    /// </summary>
    /// <remarks>
    ///     Book 3 Section 10.6
    /// </remarks>
    private readonly TerminalVerificationResult _TerminalVerificationResult;

    /// <summary>
    ///     Upon completion of terminal risk management, the terminal shall set the ‘Terminal risk management was performed’
    ///     bit in the TSI to 1.
    /// </summary>
    /// <remarks>
    ///     Book 3 Section 10.6
    /// </remarks>
    private readonly TransactionStatusInformationFlags _TransactionStatus;

    #endregion

    #region Constructor

    public TerminalRiskManagementResponse(TerminalVerificationResult terminalVerificationResult, TransactionStatusInformationFlags transactionStatus)
    {
        _TerminalVerificationResult = terminalVerificationResult;
        _TransactionStatus = transactionStatus;
    }

    #endregion

    #region Instance Members

    public TerminalVerificationResult GetTerminalVerificationResult() => _TerminalVerificationResult;
    public TransactionStatusInformationFlags GetTransactionStatusInformationFlags() => _TransactionStatus;

    #endregion
}