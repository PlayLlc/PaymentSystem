using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Enums.Interchange;

namespace Play.Emv.Kernel.Databases;

public partial class KernelDatabase
{
    #region Transaction Details

    /// <summary>
    ///     A transaction in which the cardholder receives cash from a self service kiosk or cashier
    /// </summary>
    public bool IsCashbackTransaction()
    {
        TransactionType transactionType = GetTransactionType();

        return (transactionType == TransactionTypes.CashAdvance) || (transactionType == TransactionTypes.FastCashDebit);
    }

    /// <summary>
    ///     A legacy transaction that was manually keyed by an attended
    /// </summary>
    /// <returns></returns>
    public bool IsManualTransaction()
    {
        PosEntryMode? posEntryMode = GetPosEntryMode();

        return (posEntryMode == PosEntryModes.ManualEntry) || (posEntryMode == PosEntryModes.ManualEntryFallback);
    }

    /// <summary>
    ///     A legacy transaction that was manually keyed by an attended where the cardholder receives cash at the end of the
    ///     transaction
    /// </summary>
    public bool IsManualCashTransaction() => IsCashTransaction() && IsManualTransaction();

    /// <summary>
    ///     A transaction at an unattended terminal where the cardholder receives cash, such as an ATM withdrawal
    /// </summary>
    public bool IsUnattendedCashTransaction() =>
        GetTerminalType().IsEnvironmentType(TerminalType.EnvironmentType.Unattended) && IsCashTransaction();

    /// <summary>
    ///     A purchase transaction in which the cardholder receives cash from a self service kiosk or cashier
    /// </summary>
    public bool IsPurchaseTransactionWithCashback() => IsPurchaseTransaction() && IsCashbackTransaction();

    /// <summary>
    ///     A sale of goods and services
    /// </summary>
    public bool IsPurchaseTransaction()
    {
        TransactionType transactionType = GetTransactionType();

        if (transactionType == TransactionTypes.GoodsAndServicesDebit)
            return true;

        if (transactionType == TransactionTypes.GoodsAndServicesWithCashDisbursementDebit)
            return true;

        return false;
    }

    /// <summary>
    ///     A transaction in which the cardholder receives cash, such as a quick loan, ATM withdrawal using a credit card, and
    ///     so on
    /// </summary>
    public bool IsCashTransaction()
    {
        TransactionType transactionType = GetTransactionType();

        if (transactionType == TransactionTypes.CashAdvance)
            return true;

        if (transactionType == TransactionTypes.FastCashDebit)
            return true;

        return false;
    }

    #endregion
}