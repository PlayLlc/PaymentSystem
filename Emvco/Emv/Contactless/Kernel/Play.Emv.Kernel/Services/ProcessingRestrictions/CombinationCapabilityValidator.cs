using System;

using Play.Emv.DataElements;
using Play.Emv.Kernel.Databases;

namespace Play.Emv.Kernel.Services.ProcessingRestrictions;

public class CombinationCapabilityValidator : IValidateCombinationCompatibility
{
    #region Instance Members

    /// <exception cref="Emv.Exceptions.TerminalDataException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
    public void Process(KernelDatabase database)
    {
        if (!IsApplicationCapabilityCheckPossible(database))
            return;

        if (!IsApplicationCompatibleWithTerminalType(database))
            return;

        if (!IsAdditionalCompatibilityCheckingPossible(database))
            return;

        ApplicationUsageControl applicationUsageControl =
            ApplicationUsageControl.Decode(database.Get(ApplicationUsageControl.Tag).EncodeValue().AsSpan());
        TerminalCountryCode terminalCountryCode = TerminalCountryCode.Decode(database.Get(TerminalCountryCode.Tag).EncodeValue().AsSpan());
        IssuerCountryCode issuerCountryCode = IssuerCountryCode.Decode(database.Get(IssuerCountryCode.Tag).EncodeValue().AsSpan());

        InitializeCashTransactionCompatibilityFlags(applicationUsageControl, terminalCountryCode, issuerCountryCode, database);

        InitializeCountryCompatibilityFlags(applicationUsageControl, terminalCountryCode, issuerCountryCode, database);

        InitializeCashbackCompatibilityFlags(applicationUsageControl, terminalCountryCode, issuerCountryCode, database);
    }

    #region PRE.9

    /// <exception cref="Emv.Exceptions.TerminalDataException"></exception>
    public static bool IsApplicationCapabilityCheckPossible(KernelDatabase database)
    {
        if (database.IsPresentAndNotEmpty(ApplicationUsageControl.Tag))
            return true;

        return false;
    }

    #endregion

    #region PRE.10

    /// <exception cref="Emv.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Emv.Exceptions.TerminalDataException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static bool IsTerminalAnAtm(KernelDatabase database)
    {
        AdditionalTerminalCapabilities additionalTerminal

        AdditionalTerminalCapabilities.Decode(database.Get(AdditionalTerminalCapabilities.Tag).EncodeValue().AsSpan());

        if (!additionalTerminalCapabilities.Cash())
            return false;

        TerminalType terminalType = TerminalType.Decode(database.Get(TerminalType.Tag).EncodeValue().AsSpan());

        TerminalType onTerminalType.EnvironmentTypenvironmentTypTerminalType.CommunicationTypemunicationTy
        TerminalType.TerminalOperatorTypealOperatorType.FinancialInstitution);
        TerminalType offTerminalType.EnvironmentTypenvironmentTypTerminalType.CommunicationTypemunicationTyp
        TerminalType.TerminalOperatorTypealOperatorType.FinancialInstitution);
        TerminalType onlineAndOffTerminalType.EnvironmentTypenvironmentTy
        TerminalType.CommunicationTypemunicationType.OnlineAndO
        TerminalType.TerminalOperatorTypealOperatorType.FinancialInstitution);

        if (terminalType == onlineAtm)
            return true;
        if (terminalType == offlineAtm)
            return true;
        if (terminalType == onlineAndOfflineAtm)
            return true;

        return false;
    }

    #endregion #endregion

    #endregion

    #region PRE.11 - PRE.14 Terminal Compatibility

    /// <exception cref="Emv.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="Emv.Exceptions.TerminalDataException"></exception>
    /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
    public bool IsApplicationCompatibleWithTerminalType(KernelDatabase database)
    {
        ApplicationUsageControl application

        ApplicationUsageControl.Decode(database.Get(ApplicationUsageControl.Tag).EncodeValue().AsSpan());

        if (IsTerminalAnAtm(database))
            return IsCompatibleWithAtmTerminals(database, applicationUsageControl);

        return IsCompatibleWithNonAtmTerminals(database, applicationUsageControl);
    }

    #region PRE.11

    /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
    /// <exception cref="Emv.Exceptions.TerminalDataException"></exception>
    public static bool IsCompatibleWithNonAtmTerminals(KernelDatabase database, ApplicationUsageControl applicationUsageControl)
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

    /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
    /// <exception cref="Emv.Exceptions.TerminalDataException"></exception>
    public static bool IsCompatibleWithAtmTerminals(KernelDatabase database, ApplicationUsageControl applicationUsageControl)
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

    /// <exception cref="Emv.Exceptions.TerminalDataException"></exception>
    public static bool IsAdditionalCompatibilityCheckingPossible(KernelDatabase database)
    {
        if (!database.IsPresentAndNotEmpty(IssuerCountryCode.Tag))
            return false;

        return true;
    }

    #endregion

    #endregion

    #region PRE 15 - 19 Cash Transaction Compatibility

    /// <exception cref="Emv.Exceptions.TerminalDataException"></exception>
    /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
    public void Initializ

    private actionCompatibilityFlags(ApplicationUs applicationUsageControl, Termi Code terminalCountryCode,
    private I ryCode issuerCountryCode,
    private KernelDatabase database)
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

    public static bool IsCashTransaction(KernelDatabase database)
    {
        TransactionType transactionType = TransactionType.Decode(database.Get(TransactionType.Tag).EncodeValue().AsSpan());

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

    /// <exception cref="Emv.Exceptions.TerminalDataException"></exception>
    /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
    private static void CheckIfDomesticCashTransactionIsAllowed(ApplicationUsageControl applicationUsageControl, KernelDatabase database)
    {
        if (!applicationUsageControl.IsValidForDomesticCashTransactions())
            database.Set(TerminalVerificationResultCodes.RequestedServiceNotAllowedForCardProduct);
    }

    #endregion

    #region PRE.18, PRE.19

    /// <exception cref="Emv.Exceptions.TerminalDataException"></exception>
    /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
    private static void CheckIfInternationalCashTransactionIsAllowed(
        ApplicationUsageControl applicationUsageControl,
        KernelDatabase database)
    {
        if (!applicationUsageControl.IsValidForInternationalCashTransactions())
            database.Set(TerminalVerificationResultCodes.RequestedServiceNotAllowedForCardProduct);
    }

    #endregion

    #endregion

    #region PRE.20 - 24 Country Compatibility

    /// <exception cref="Emv.Exceptions.TerminalDataException"></exception>
    /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
    /// <exception cref="Emv.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exceptio
    blic

    private void InitializeCountryCompatibilityFlags(plicationUsageControl applicationUsageCo
    private TerminalCountryCode terminalCo
    private IssuerCountryCode issuerCountryCode, KernelDatabase database)
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

    /// <exception cref="Emv.Exceptions.TerminalDataException"></exception>
    /// <exception cref="Emv.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static bool IsPurchaseTransaction(KernelDatabase database)
    {
        TransactionType transactionType = TransactionType.Decode(database.Get(TransactionType.Tag).EncodeValue().AsSpan());

        if (transactionType == TransactionTypes.GoodsAndServicesDebit)
            return true;

        if (transactionType == TransactionTypes.GoodsAndServicesWithCashDisbursementDebit)
            return true;

        return false;
    }

    #endregion

    #region PRE.22, PRE.24

    /// <exception cref="Emv.Exceptions.TerminalDataException"></exception>
    /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
    private static void CheckIfDomesticGoodsAndServicesAreAllowed(ApplicationUsageControl applicationUsageControl, KernelDatabase database)
    {
        if (applicationUsageControl.IsValidForDomesticGoods())
            return;

        if (applicationUsageControl.IsValidForDomesticServices())
            return;

        database.Set(TerminalVerificationResultCodes.RequestedServiceNotAllowedForCardProduct);
    }

    #endregion

    #region PRE.23, PRE.24

    /// <exception cref="Emv.Exceptions.TerminalDataException"></exception>
    /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
    private static void CheckIfInternationalGoodsAndServicesAreAllowed(
        ApplicationUsageControl applicationUsageControl,
        KernelDatabase database)
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

    /// <exception cref="Emv.Exceptions.TerminalDataException"></exception>
    /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
    /// <exception cref="Emv.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingE
    </exception>
    public void InitializeCashback

    private ityFlags(
        ApplicationUsageContr tionUsageControl,
        TerminalC terminalCountryCode,
        IssuerCountryCode issuerCountryCode,
        KernelDatabase database)
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

    /// <exception cref="Emv.Exceptions.TerminalDataException"></exception>
    /// <exception cref="Emv.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static bool IsCashbackRequested(KernelDatabase database)
    {
        if (database.IsPresentAndNotEmpty(AmountOtherNumeric.Tag))
            return true;

        return false;
    }

    #endregion

    #region PRE.27, PRE.29

    /// <exception cref="Emv.Exceptions.TerminalDataException"></exception>
    /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
    private static void CheckIfDomesticCashbackIsAllowed(ApplicationUsageControl applicationUsageControl, KernelDatabase database)
    {
        if (applicationUsageControl.IsDomesticCashbackAllowed())
            return;

        database.Set(TerminalVerificationResultCodes.RequestedServiceNotAllowedForCardProduct);
    }

    #endregion

    #region PRE.28, PRE.29

    /// <exception cref="Emv.Exceptions.TerminalDataException"></exception>
    /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
    private static void CheckIfInternationalCashbackIsAllowed(ApplicationUsageControl applicationUsageControl, KernelDatabase database)
    {
        if (applicationUsageControl.IsInternationalCashbackAllowed())
            return;

        database.Set(TerminalVerificatio
        esultCodes.RequestedServiceNotAllowedForCardProduct);
    }

    #endregion

    #endregion
}