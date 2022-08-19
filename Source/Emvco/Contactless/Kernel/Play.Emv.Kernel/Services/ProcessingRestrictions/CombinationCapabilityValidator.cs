using System;

using Play.Codecs.Exceptions;
using Play.Core.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Kernel.Databases;

namespace Play.Emv.Kernel.Services;

public class CombinationCapabilityValidator : IValidateCombinationCompatibility
{
    #region Instance Members

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="PlayInternalException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public void Process(KernelDatabase database)
    {
        if (!IsApplicationCapabilityCheckPossible(database))
            return;

        if (!IsApplicationCompatibleWithTerminalType(database))
            return;

        if (!IsAdditionalCompatibilityCheckingPossible(database))
            return;

        ApplicationUsageControl applicationUsageControl = database.Get<ApplicationUsageControl>(ApplicationUsageControl.Tag);
        TerminalCountryCode terminalCountryCode = database.Get<TerminalCountryCode>(TerminalCountryCode.Tag);
        IssuerCountryCode issuerCountryCode = database.Get<IssuerCountryCode>(IssuerCountryCode.Tag);

        InitializeCashTransactionCompatibilityFlags(applicationUsageControl, terminalCountryCode, issuerCountryCode, database);

        InitializeCountryCompatibilityFlags(applicationUsageControl, terminalCountryCode, issuerCountryCode, database);

        InitializeCashbackCompatibilityFlags(applicationUsageControl, terminalCountryCode, issuerCountryCode, database);
    }

    #region PRE.9

    /// <exception cref="TerminalDataException"></exception>
    private bool IsApplicationCapabilityCheckPossible(KernelDatabase database)
    {
        if (database.IsPresentAndNotEmpty(ApplicationUsageControl.Tag))
            return true;

        return false;
    }

    #endregion

    #region PRE.10

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private bool IsTerminalAnAtm(KernelDatabase database)
    {
        AdditionalTerminalCapabilities additionalTerminalCapabilities = database.Get<AdditionalTerminalCapabilities>(AdditionalTerminalCapabilities.Tag);

        if (!additionalTerminalCapabilities.Cash())
            return false;

        TerminalType terminalType = database.Get<TerminalType>(TerminalType.Tag);

        TerminalType onlineAtm = new(TerminalType.EnvironmentType.Unattended, TerminalType.CommunicationType.OnlineOnly,
            TerminalType.TerminalOperatorType.FinancialInstitution);
        TerminalType offlineAtm = new(TerminalType.EnvironmentType.Unattended, TerminalType.CommunicationType.OfflineOnly,
            TerminalType.TerminalOperatorType.FinancialInstitution);
        TerminalType onlineAndOfflineAtm = new(TerminalType.EnvironmentType.Unattended, TerminalType.CommunicationType.OnlineAndOfflineCapable,
            TerminalType.TerminalOperatorType.FinancialInstitution);

        if (terminalType == onlineAtm)
            return true;
        if (terminalType == offlineAtm)
            return true;
        if (terminalType == onlineAndOfflineAtm)
            return true;

        return false;
    }

    #endregion

    #endregion

    #region PRE.11 - PRE.14 Terminal Compatibility

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="PlayInternalException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private bool IsApplicationCompatibleWithTerminalType(KernelDatabase database)
    {
        ApplicationUsageControl applicationUsageControl = database.Get<ApplicationUsageControl>(ApplicationUsageControl.Tag);

        if (IsTerminalAnAtm(database))
            return IsCompatibleWithAtmTerminals(database, applicationUsageControl);

        return IsCompatibleWithNonAtmTerminals(database, applicationUsageControl);
    }

    #region PRE.11

    /// <exception cref="PlayInternalException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    private bool IsCompatibleWithNonAtmTerminals(KernelDatabase database, ApplicationUsageControl applicationUsageControl)
    {
        if (!applicationUsageControl.IsValidAtTerminalsOtherThanAtms())
        {
            // PRE.13
            database.Set(TerminalVerificationResultCodes.RequestedServiceNotAllowedForCardProduct);

            return false;
        }

        return true;
    }

    #endregion

    #region PRE.12

    /// <exception cref="PlayInternalException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    private bool IsCompatibleWithAtmTerminals(KernelDatabase database, ApplicationUsageControl applicationUsageControl)
    {
        if (!applicationUsageControl.IsValidAtAtms())
        {
            // PRE.13
            database.Set(TerminalVerificationResultCodes.RequestedServiceNotAllowedForCardProduct);

            return false;
        }

        return true;
    }

    #endregion

    #region PRE.13

    // Set inside methods

    #endregion

    #region PRE.14

    /// <exception cref="TerminalDataException"></exception>
    private bool IsAdditionalCompatibilityCheckingPossible(KernelDatabase database)
    {
        if (!database.IsPresentAndNotEmpty(IssuerCountryCode.Tag))
            return false;

        return true;
    }

    #endregion

    #endregion

    #region PRE 15 - 19 Cash Transaction Compatibility

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="PlayInternalException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private void InitializeCashTransactionCompatibilityFlags(
        ApplicationUsageControl applicationUsageControl, TerminalCountryCode terminalCountryCode, IssuerCountryCode issuerCountryCode, KernelDatabase database)
    {
        if (!IsCashTransaction(database))
            return;

        // PRE.16
        if (terminalCountryCode == issuerCountryCode)
            CheckIfDomesticCashTransactionIsAllowed(applicationUsageControl, database);
        else
            CheckIfInternationalCashTransactionIsAllowed(applicationUsageControl, database);
    }

    #region PRE.15

    /// <summary>
    ///     IsCashTransaction
    /// </summary>
    /// <param name="database"></param>
    /// <returns></returns>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private bool IsCashTransaction(KernelDatabase database)
    {
        TransactionType transactionType = database.Get<TransactionType>(TransactionType.Tag);

        if (transactionType == TransactionTypes.CashAdvance)
            return true;

        if (transactionType == TransactionTypes.FastCashDebit)
            return true;

        return false;
    }

    #endregion

    #region PRE.16

    // PRE.16 is handled inside InitializeCurrencyCompatibilityFlags

    #endregion

    #region PRE.17, PRE.19

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="PlayInternalException"></exception>
    private void CheckIfDomesticCashTransactionIsAllowed(ApplicationUsageControl applicationUsageControl, KernelDatabase database)
    {
        if (!applicationUsageControl.IsValidForDomesticCashTransactions())
            database.Set(TerminalVerificationResultCodes.RequestedServiceNotAllowedForCardProduct);
    }

    #endregion

    #region PRE.18, PRE.19

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="PlayInternalException"></exception>
    private void CheckIfInternationalCashTransactionIsAllowed(ApplicationUsageControl applicationUsageControl, KernelDatabase database)
    {
        if (!applicationUsageControl.IsValidForInternationalCashTransactions())
            database.Set(TerminalVerificationResultCodes.RequestedServiceNotAllowedForCardProduct);
    }

    #endregion

    #endregion

    #region PRE.20 - 24 Country Compatibility

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="PlayInternalException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private void InitializeCountryCompatibilityFlags(
        ApplicationUsageControl applicationUsageControl, TerminalCountryCode terminalCountryCode, IssuerCountryCode issuerCountryCode, KernelDatabase database)
    {
        if (!IsPurchaseTransaction(database))
            return;

        // PRE.16
        if (terminalCountryCode == issuerCountryCode)
            CheckIfDomesticGoodsAndServicesAreAllowed(applicationUsageControl, database);
        else
            CheckIfInternationalGoodsAndServicesAreAllowed(applicationUsageControl, database);
    }

    #region PRE.20

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private bool IsPurchaseTransaction(KernelDatabase database)
    {
        TransactionType transactionType = database.Get<TransactionType>(TransactionType.Tag);

        if (transactionType == TransactionTypes.GoodsAndServicesDebit)
            return true;

        if (transactionType == TransactionTypes.GoodsAndServicesWithCashDisbursementDebit)
            return true;

        return false;
    }

    #endregion

    #region PRE.22, PRE.24

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="PlayInternalException"></exception>
    private void CheckIfDomesticGoodsAndServicesAreAllowed(ApplicationUsageControl applicationUsageControl, KernelDatabase database)
    {
        if (applicationUsageControl.IsValidForDomesticGoods())
            return;

        if (applicationUsageControl.IsValidForDomesticServices())
            return;

        database.Set(TerminalVerificationResultCodes.RequestedServiceNotAllowedForCardProduct);
    }

    #endregion

    #region PRE.23, PRE.24

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="PlayInternalException"></exception>
    private void CheckIfInternationalGoodsAndServicesAreAllowed(ApplicationUsageControl applicationUsageControl, KernelDatabase database)
    {
        if (applicationUsageControl.IsValidForInternationalGoods())
            return;

        if (applicationUsageControl.IsValidForInternationalServices())
            return;

        database.Set(TerminalVerificationResultCodes.RequestedServiceNotAllowedForCardProduct);
    }

    #endregion

    #endregion

    #region PRE.25 - PRE.29 Cashback Compatibility

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="PlayInternalException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    private void InitializeCashbackCompatibilityFlags(
        ApplicationUsageControl applicationUsageControl, TerminalCountryCode terminalCountryCode, IssuerCountryCode issuerCountryCode, KernelDatabase database)
    {
        if (!IsCashbackRequested(database))
            return;

        // PRE.26
        if (terminalCountryCode == issuerCountryCode)
            CheckIfDomesticCashbackIsAllowed(applicationUsageControl, database);
        else
            CheckIfInternationalCashbackIsAllowed(applicationUsageControl, database);
    }

    #region PRE.25

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    private bool IsCashbackRequested(KernelDatabase database)
    {
        if (database.IsPresentAndNotEmpty(AmountOtherNumeric.Tag))
            return true;

        return false;
    }

    #endregion

    #region PRE.27, PRE.29

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="PlayInternalException"></exception>
    private void CheckIfDomesticCashbackIsAllowed(ApplicationUsageControl applicationUsageControl, KernelDatabase database)
    {
        if (applicationUsageControl.IsDomesticCashbackAllowed())
            return;

        database.Set(TerminalVerificationResultCodes.RequestedServiceNotAllowedForCardProduct);
    }

    #endregion

    #region PRE.28, PRE.29

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="PlayInternalException"></exception>
    private void CheckIfInternationalCashbackIsAllowed(ApplicationUsageControl applicationUsageControl, KernelDatabase database)
    {
        if (applicationUsageControl.IsInternationalCashbackAllowed())
            return;

        database.Set(TerminalVerificationResultCodes.RequestedServiceNotAllowedForCardProduct);
    }

    #endregion

    #endregion
}