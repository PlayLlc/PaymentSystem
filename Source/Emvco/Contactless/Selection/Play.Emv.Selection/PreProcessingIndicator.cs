#nullable enable
using Play.Emv.Ber.DataElements;
using Play.Emv.Identifiers;
using Play.Globalization;
using Play.Globalization.Currency;
using Play.Icc.FileSystem.DedicatedFiles;

using TransactionProfile = Play.Emv.Selection.Contracts.TransactionProfile;

namespace Play.Emv.Selection;

/// <summary>
///     An indicator specific to the Reader Combination selected will be provided to the Kernel to aid in processing
/// </summary>
/// <remarks>
///     These are going to be mutable singleton instances during the lifetime of the application. The configuration updates
///     are an exceptional occurrence that will happen outside of transaction processing.
/// </remarks>
public class PreProcessingIndicator
{
    #region Instance Values

    public bool ContactlessApplicationNotAllowed;
    public bool ReaderContactlessFloorLimitExceeded;
    public bool ReaderCvmRequiredLimitExceeded;
    public bool StatusCheckRequested;
    public TerminalTransactionQualifiers TerminalTransactionQualifiers;
    public bool ZeroAmount;
    private readonly CombinationCompositeKey _CombinationCompositeKey;
    private readonly TransactionProfile _TransactionProfile;

    #endregion

    #region Constructor

    public PreProcessingIndicator(TransactionProfile transactionProfile)
    {
        _TransactionProfile = transactionProfile;
        _CombinationCompositeKey = transactionProfile.GetCombinationCompositeKey();
        TerminalTransactionQualifiers = _TransactionProfile.GetTerminalTransactionQualifiers().AsValueCopy();
    }

    #endregion

    #region Instance Members

    public PreProcessingIndicatorResult AsPreProcessingIndicatorResult() => new(_CombinationCompositeKey, TerminalTransactionQualifiers);
    public DedicatedFileName GetApplicationIdentifier() => _CombinationCompositeKey.GetApplicationId();
    public KernelId GetKernelId() => _CombinationCompositeKey.GetKernelId();
    public CombinationCompositeKey GetKey() => _CombinationCompositeKey;
    public bool IsExtendedSelectionSupported() => _TransactionProfile.IsExtendedSelectionSupported();
    public bool IsMatchingKernel(KernelIdentifier kernelIdentifier) => GetKernelId().Equals(kernelIdentifier.AsKernelId());

    /// <summary>
    ///     This will clear <see cref="TerminalTransactionQualifiers" /> and <see cref="PreProcessingIndicator" /> objects
    /// </summary>
    public void ResetPreprocessingIndicators()
    {
        ContactlessApplicationNotAllowed = false;
        ReaderContactlessFloorLimitExceeded = false;
        ReaderCvmRequiredLimitExceeded = false;
        StatusCheckRequested = false;
        ZeroAmount = false;
    }

    public void ResetTerminalTransactionQualifiers() => _TransactionProfile.GetTerminalTransactionQualifiers().AsValueCopy();

    /// <summary>
    ///     This will set All fields in accordance to Start A Preprocessing
    /// </summary>
    /// <param name="transactionSpecificDataElements">
    ///     Represents transaction specific information that the chosen Kernel will need to process the transaction. Each
    ///     Kernel will have their own concrete implementation
    /// </param>
    /// <param name="amountAuthorizedNumeric"></param>
    /// <param name="cultureProfile"></param>
    /// <remarks>
    ///     Emv Book B Section 3.1.1.2
    /// </remarks>
    public void Set(AmountAuthorizedNumeric amountAuthorizedNumeric, CultureProfile cultureProfile)
    {
        ResetPreprocessingIndicators();
        ResetTerminalTransactionQualifiers();
        SetMutableFields(amountAuthorizedNumeric, cultureProfile);
    }

    /// <remarks>
    ///     Emv Book B Section 3.1.1.11
    /// </remarks>
    private void SetContactlessApplicationNotAllowed(ZeroAmountHasBeenSetEvent zeroAmountHasBeenSetEvent)
    {
        if (zeroAmountHasBeenSetEvent.ZeroAmount && TerminalTransactionQualifiers.IsReaderOfflineOnly())
            ContactlessApplicationNotAllowed = true;
    }

    /// <remarks>
    ///     Emv Book B Section 3.1.1.4
    /// </remarks>
    private void ProcessZeroAmountCondition(Money amountAuthorizedNumeric, bool isZeroAmountAllowedForOffline)
    {
        if (!amountAuthorizedNumeric.IsZeroAmount())
            return;

        if (isZeroAmountAllowedForOffline)
            return;

        ContactlessApplicationNotAllowed = true;
    }

    /// <remarks>
    ///     Emv Book B Section 3.1.1.5
    /// </remarks>
    public void ProcessReaderContactlessTransactionLimit(Money amountAuthorizedNumeric, Money readerContactlessTransactionLimit)
    {
        if (amountAuthorizedNumeric >= readerContactlessTransactionLimit)
            ContactlessApplicationNotAllowed = true;
    }

    /// <remarks>
    ///     Emv Book B Section 3.1.1.12
    /// </remarks>
    private void SetCvmRequired(ReaderCvmRequiredLimitExceededHasBeenSetEvent readerCvmRequiredLimitExceededHasBeenSetEvent)
    {
        if (readerCvmRequiredLimitExceededHasBeenSetEvent.ReaderCvmRequiredLimitExceeded)
            TerminalTransactionQualifiers.SetCvmRequired();
    }

    // i think this is process A, anyone want to help me out here? i accept wage free indentured servs! 
    private void SetMutableFields(AmountAuthorizedNumeric amountAuthorizedNumeric, CultureProfile cultureProfile)
    {
        Money amountAuthorizedMoney = amountAuthorizedNumeric.AsMoney(cultureProfile.GetNumericCurrencyCode());

        StatusCheckRequestedHasBeenSetEvent? statusCheckRequestedHasBeenSet =
            SetStatusCheckRequested(_TransactionProfile.IsStatusCheckSupported(), amountAuthorizedMoney);
        ZeroAmountHasBeenSetEvent? zeroAmountHasBeenSet = SetZeroAmount(amountAuthorizedMoney, _TransactionProfile.IsZeroAmountAllowedForOffline());
        ProcessZeroAmountCondition(amountAuthorizedMoney, _TransactionProfile.IsZeroAmountAllowedForOffline());
        ProcessReaderContactlessTransactionLimit(amountAuthorizedMoney,
            _TransactionProfile.GetReaderContactlessTransactionLimit().AsMoney(cultureProfile.GetNumericCurrencyCode()));

        ReaderContactlessFloorLimitExceededHasBeenSetEvent? readerContactlessFloorLimitExceededHasBeenSet =
            SetReaderContactlessFloorLimitExceeded(amountAuthorizedMoney,
                _TransactionProfile.GetReaderContactlessTransactionLimit().AsMoney(cultureProfile.GetNumericCurrencyCode()));
        ReaderCvmRequiredLimitExceededHasBeenSetEvent? readerCvmRequiredLimitExceeded = SetReaderCvmRequiredLimitExceeded(amountAuthorizedMoney,
            _TransactionProfile.GetReaderCvmRequiredLimit().AsMoney(cultureProfile.GetNumericCurrencyCode()));
        SetOnlineCryptogramRequired(readerContactlessFloorLimitExceededHasBeenSet);
        SetOnlineCryptogramRequired(statusCheckRequestedHasBeenSet);
        SetOnlineCryptogramRequired(zeroAmountHasBeenSet);
        SetContactlessApplicationNotAllowed(zeroAmountHasBeenSet);
        SetCvmRequired(readerCvmRequiredLimitExceeded);
    }

    /// <remarks>
    ///     Emv Book B Section 3.1.1.11
    /// </remarks>
    private void SetOnlineCryptogramRequired(ZeroAmountHasBeenSetEvent zeroAmountHasBeenSetEvent)
    {
        if (zeroAmountHasBeenSetEvent.ZeroAmount && TerminalTransactionQualifiers.IsReaderOnlineCapable())
            TerminalTransactionQualifiers.SetOnlineCryptogramRequired();

        // TODO SET Contactless App not allowed
    }

    /// <remarks>
    ///     Emv Book B Section 3.1.1.10
    /// </remarks>
    private void SetOnlineCryptogramRequired(StatusCheckRequestedHasBeenSetEvent statusCheckRequestedHasBeenSetEvent)
    {
        if (statusCheckRequestedHasBeenSetEvent.StatusCheckRequested)
            TerminalTransactionQualifiers.SetOnlineCryptogramRequired();
    }

    /// <remarks>
    ///     Emv Book B Section 3.1.1.9
    /// </remarks>
    private void SetOnlineCryptogramRequired(ReaderContactlessFloorLimitExceededHasBeenSetEvent readerContactlessFloorLimitExceededHasBeenSetEvent)
    {
        if (readerContactlessFloorLimitExceededHasBeenSetEvent.ReaderContactlessFloorLimitExceeded)
            TerminalTransactionQualifiers.SetOnlineCryptogramRequired();
    }

    // TODO: Disabled this section because of instead of making all configurable fields nullable I'm using default values and assuming that's okay. Keeping this here
    // TODO: until I confirm that is okay
    ///// <remarks>
    /////     Emv Book B Section 3.1.1.7
    ///// </remarks>
    //public void SetReaderContactlessFloorLimitExceeded2(AmountAuthorizedNumeric amountAuthorizedNumeric, TerminalFloorLimit terminalFloorLimit)
    //{
    //    if (amountAuthorizedNumeric > terminalFloorLimit)
    //        ReaderContactlessFloorLimitExceeded = true;
    //}

    /// <remarks>
    ///     Emv Book B Section 3.1.1.6
    /// </remarks>
    private ReaderContactlessFloorLimitExceededHasBeenSetEvent SetReaderContactlessFloorLimitExceeded(
        Money amountAuthorizedNumeric, Money readerContactlessFloorLimit)
    {
        if (amountAuthorizedNumeric > readerContactlessFloorLimit)
            ReaderContactlessFloorLimitExceeded = true;

        return new ReaderContactlessFloorLimitExceededHasBeenSetEvent(ReaderContactlessFloorLimitExceeded);
    }

    /// <remarks>
    ///     Emv Book B Section 3.1.1.8
    /// </remarks>
    private ReaderCvmRequiredLimitExceededHasBeenSetEvent SetReaderCvmRequiredLimitExceeded(Money amountAuthorizedNumeric, Money readerCvmRequiredLimit)
    {
        if (amountAuthorizedNumeric > readerCvmRequiredLimit)
            ReaderCvmRequiredLimitExceeded = true;

        return new ReaderCvmRequiredLimitExceededHasBeenSetEvent(ReaderCvmRequiredLimitExceeded);
    }

    /// <remarks>
    ///     Emv Book B Section 3.1.1.3
    /// </remarks>
    private StatusCheckRequestedHasBeenSetEvent SetStatusCheckRequested(bool isStatusCheckSupported, Money amountAuthorizedNumeric)
    {
        if (isStatusCheckSupported && amountAuthorizedNumeric.IsBaseAmount())
            StatusCheckRequested = true;

        return new StatusCheckRequestedHasBeenSetEvent(StatusCheckRequested);
    }

    /// <remarks>
    ///     Emv Book B Section 3.1.1.4
    /// </remarks>
    private ZeroAmountHasBeenSetEvent SetZeroAmount(Money amountAuthorizedNumeric, bool isZeroAmountAllowedForOffline)
    {
        if (!amountAuthorizedNumeric.IsZeroAmount())
            return new ZeroAmountHasBeenSetEvent(ZeroAmount);

        if (isZeroAmountAllowedForOffline)
            ZeroAmount = true;

        return new ZeroAmountHasBeenSetEvent(ZeroAmount);
    }

    #endregion

    private class ReaderCvmRequiredLimitExceededHasBeenSetEvent
    {
        #region Instance Values

        public readonly bool ReaderCvmRequiredLimitExceeded;

        #endregion

        #region Constructor

        public ReaderCvmRequiredLimitExceededHasBeenSetEvent(bool readerCvmRequiredLimitExceeded)
        {
            ReaderCvmRequiredLimitExceeded = readerCvmRequiredLimitExceeded;
        }

        #endregion
    }

    private class StatusCheckRequestedHasBeenSetEvent
    {
        #region Instance Values

        public readonly bool StatusCheckRequested;

        #endregion

        #region Constructor

        public StatusCheckRequestedHasBeenSetEvent(bool statusCheckRequested)
        {
            StatusCheckRequested = statusCheckRequested;
        }

        #endregion
    }

    private class ReaderContactlessFloorLimitExceededHasBeenSetEvent
    {
        #region Instance Values

        public readonly bool ReaderContactlessFloorLimitExceeded;

        #endregion

        #region Constructor

        public ReaderContactlessFloorLimitExceededHasBeenSetEvent(bool readerContactlessFloorLimitExceeded)
        {
            ReaderContactlessFloorLimitExceeded = readerContactlessFloorLimitExceeded;
        }

        #endregion
    }

    private class ZeroAmountHasBeenSetEvent
    {
        #region Instance Values

        public readonly bool ZeroAmount;

        #endregion

        #region Constructor

        public ZeroAmountHasBeenSetEvent(bool zeroAmount)
        {
            ZeroAmount = zeroAmount;
        }

        #endregion
    }
}