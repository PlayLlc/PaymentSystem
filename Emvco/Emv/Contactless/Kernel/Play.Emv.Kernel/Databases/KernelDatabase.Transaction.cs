using System;

using Play.Codecs.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Enums.Interchange;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Kernel.Databases;

public partial class KernelDatabase
{
    #region Instance Values

    protected MobileSupportIndicator.Builder _MobileSupportIndicatorBuilder = MobileSupportIndicator.GetBuilder();

    #endregion

    #region Write

    /// <exception cref="TerminalDataException"></exception>
    public MobileSupportIndicator GetMobileSupportIndicator()
    {
        if (TryGet(MobileSupportIndicator.Tag, out MobileSupportIndicator? mobileSupportIndicator))
            return mobileSupportIndicator;

        MobileSupportIndicator? defaultMobileSupportIndicator = _MobileSupportIndicatorBuilder.Complete();
        Update(defaultMobileSupportIndicator);

        return defaultMobileSupportIndicator;
    }

    /// <exception cref="TerminalDataException"></exception>
    public void SetMobileSupported(bool value)
    {
        try
        {
            _MobileSupportIndicatorBuilder.Reset(GetMobileSupportIndicator());
            _MobileSupportIndicatorBuilder.SetMobileSupported(value);
            Update(_MobileSupportIndicatorBuilder.Complete());
        }
        catch (DataElementParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(MobileSupportIndicator)}", exception);
        }
        catch (CodecParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(MobileSupportIndicator)}", exception);
        }
        catch (Exception exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(MobileSupportIndicator)}", exception);
        }
    }

    /// <exception cref="TerminalDataException"></exception>
    public void SetOnDeviceCvmRequired(bool value)
    {
        try
        {
            _MobileSupportIndicatorBuilder.Reset(GetMobileSupportIndicator());
            _MobileSupportIndicatorBuilder.SetOnDeviceCvmRequired(value);
            Update(_MobileSupportIndicatorBuilder.Complete());
        }
        catch (DataElementParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(MobileSupportIndicator)}", exception);
        }
        catch (CodecParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(MobileSupportIndicator)}", exception);
        }
        catch (Exception exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(MobileSupportIndicator)}", exception);
        }
    }

    #endregion

    #region Read

    /// <summary>
    ///     A transaction in which the cardholder receives cash from a self service kiosk or cashier
    /// </summary>
    /// <exception cref="TerminalDataException"></exception>
    public bool IsCashbackTransaction()
    {
        TransactionType transactionType = GetTransactionType();

        return (transactionType == TransactionTypes.CashAdvance) || (transactionType == TransactionTypes.FastCashDebit);
    }

    /// <summary>
    ///     A legacy transaction that was manually keyed by an attended
    /// </summary>
    /// <returns></returns>
    /// <exception cref="TerminalDataException"></exception>
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
    /// <exception cref="TerminalDataException"></exception>
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
    /// <exception cref="TerminalDataException"></exception>
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