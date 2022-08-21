using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Identifiers;
using Play.Icc.FileSystem.DedicatedFiles;

namespace Play.Emv.Selection.Configuration;

/// <summary>
///     Configuration values for a specific Transaction/Kernel/Application combination for Entry Point processing.
///     There is an instance of this <see cref="TransactionProfile" /> for each possible combination that is
///     supported by the Terminal
/// </summary>
public class TransactionProfile : IEquatable<TransactionProfile>, IEqualityComparer<TransactionProfile>
{
    #region Instance Values

    private readonly CombinationCompositeKey _Key;
    private readonly ApplicationPriorityIndicator _ApplicationPriorityIndicator;
    private readonly ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice _ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice;
    private readonly ReaderContactlessTransactionLimitWhenCvmIsOnDevice _ReaderContactlessTransactionLimitWhenCvmIsOnDevice;

    // BUG: We should have both of the derived ReaderContactlessTransactionLimit types in the TransactionProfileDto
    private readonly ReaderContactlessFloorLimit _ReaderContactlessFloorLimit;
    private readonly ReaderCvmRequiredLimit _ReaderCvmRequiredLimit;
    private readonly KernelConfiguration _KernelConfiguration;
    private readonly TerminalTransactionQualifiers _TerminalTransactionQualifiers;

    /// <summary>
    ///     Option within the terminal related to the checking of a single unit of currency.A single unit of currency has the
    ///     value of 1 of the(major) unit of currency as defined in [ISO 4217]. As an example a single unit of currency for
    ///     Euro is 1.00.
    /// </summary>
    private readonly bool _IsStatusCheckSupported;

    private readonly bool _IsZeroAmountAllowed;
    private readonly bool _IsZeroAmountAllowedForOffline;
    private readonly bool _IsExtendedSelectionSupported;

    #endregion

    #region Constructor

    public TransactionProfile(
        CombinationCompositeKey key, ApplicationPriorityIndicator applicationPriorityIndicator,
        ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice readerContactlessTransactionLimitWhenCvmIsNotOnDevice,
        ReaderContactlessTransactionLimitWhenCvmIsOnDevice readerContactlessTransactionLimitWhenCvmIsOnDevice,
        ReaderContactlessFloorLimit readerContactlessFloorLimit, ReaderCvmRequiredLimit readerCvmRequiredLimit,
        TerminalTransactionQualifiers terminalTransactionQualifiers, KernelConfiguration kernelConfiguration, bool isStatusCheckSupported,
        bool isZeroAmountAllowed, bool isZeroAmountAllowedForOffline, bool isExtendedSelectionSupported)
    {
        _Key = key;
        _ReaderContactlessFloorLimit = readerContactlessFloorLimit;

        _KernelConfiguration = kernelConfiguration;

        _ApplicationPriorityIndicator = applicationPriorityIndicator;
        _ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice = readerContactlessTransactionLimitWhenCvmIsNotOnDevice;
        _ReaderContactlessTransactionLimitWhenCvmIsOnDevice = readerContactlessTransactionLimitWhenCvmIsOnDevice;
        _ReaderCvmRequiredLimit = readerCvmRequiredLimit;
        _IsStatusCheckSupported = isStatusCheckSupported;
        _IsZeroAmountAllowed = isZeroAmountAllowed;
        _IsZeroAmountAllowedForOffline = isZeroAmountAllowedForOffline;
        _IsExtendedSelectionSupported = isExtendedSelectionSupported;
        _TerminalTransactionQualifiers = terminalTransactionQualifiers;
    }

    #endregion

    #region Instance Members

    public KernelConfiguration GetKernelConfiguration() => _KernelConfiguration;
    public ReaderContactlessFloorLimit GetReaderContactlessFloorLimit() => _ReaderContactlessFloorLimit;
    public DedicatedFileName GetApplicationIdentifier() => _Key.GetApplicationId();
    public ApplicationPriorityIndicator GetApplicationPriorityIndicator() => _ApplicationPriorityIndicator;
    public ApplicationPriorityRank GetApplicationPriorityRank() => _ApplicationPriorityIndicator.GetApplicationPriorityRank();
    public ShortKernelIdTypes GetKernelId() => _Key.GetKernelId();
    public CombinationCompositeKey GetKey() => _Key;

    public ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice GetReaderContactlessTransactionLimitWhenCvmIsNotOnDevice() =>
        _ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice;

    public ReaderContactlessTransactionLimitWhenCvmIsOnDevice GetReaderContactlessTransactionLimitWhenCvmIsOnDevice() =>
        _ReaderContactlessTransactionLimitWhenCvmIsOnDevice;

    public ReaderCvmRequiredLimit GetReaderCvmRequiredLimit() => _ReaderCvmRequiredLimit;
    public TerminalTransactionQualifiers GetTerminalTransactionQualifiers() => _TerminalTransactionQualifiers;
    public TransactionType GetTransactionType() => _Key.GetTransactionType();
    public bool IsExtendedSelectionSupported() => _IsExtendedSelectionSupported;
    public bool IsStatusCheckSupported() => _IsStatusCheckSupported;
    public bool IsZeroAmountAllowed() => _IsZeroAmountAllowed;
    public bool IsZeroAmountAllowedForOffline() => _IsZeroAmountAllowedForOffline;

    #endregion

    #region Equality

    public bool Equals(TransactionProfile? other)
    {
        if (other == null)
            return false;

        return _Key == other._Key;
    }

    public bool Equals(TransactionProfile? x, TransactionProfile? y)
    {
        if (x == null)
            return y == null;

        return (y != null) && x.Equals(y);
    }

    public override bool Equals(object? obj) => obj is TransactionProfile transactionProfile && Equals(transactionProfile);
    public int GetHashCode(TransactionProfile obj) => obj.GetHashCode();
    public override int GetHashCode() => _Key.GetHashCode();

    #endregion

    //public PoiInformation GetPoiInformation(BerCodec codec) =>
}