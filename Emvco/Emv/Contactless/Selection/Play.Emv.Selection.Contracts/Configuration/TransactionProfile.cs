using Play.Emv.DataElements;
using Play.Emv.DataElements.Emv;
using Play.Icc.FileSystem.DedicatedFiles;

namespace Play.Emv.Selection.Contracts;

/// <summary>
///     Configuration values for a specific Transaction/Kernel/Application combination for Entry Point processing.
///     There is an instance of this <see cref="TransactionProfile" /> for each possible combination that is
///     supported by the Terminal. The complete list of these values are held in <see cref="EntryPointConfiguration" />
/// </summary>
public class TransactionProfile : IEquatable<TransactionProfile>, IEqualityComparer<TransactionProfile>
{
    #region Instance Values

    private readonly ApplicationPriorityIndicator _ApplicationPriorityIndicator;
    private readonly bool _IsExtendedSelectionSupported;
    private readonly MerchantIdentifier _MerchantIdentifier;
    private readonly TerminalIdentification _TerminalIdentification;
    private readonly InterfaceDeviceSerialNumber _InterfaceDeviceSerialNumber;
    private readonly CombinationCompositeKey _Key;
    private readonly ReaderContactlessTransactionLimit _ReaderContactlessTransactionLimit;
    private readonly ReaderCvmRequiredLimit _ReaderCvmRequiredLimit;
    private readonly TerminalCategoriesSupportedList _TerminalCategoriesSupportedList;

    // BUG: This already exists in the Terminal Configuration, no need to have TerminalFloorLimit here as well
    private readonly TerminalFloorLimit _TerminalFloorLimit;

    // BUG: TerminalTransactionQualifiers is a transient value. This should live in the Transaction Session, not here on the Transaction Profile
    private readonly TerminalTransactionQualifiers _TerminalTransactionQualifiers;

    /// <summary>
    ///     Option within the terminal related to the checking of a single unit of currency.A single unit of currency has the
    ///     value of 1 of the(major) unit of currency as defined in [ISO 4217]. As an example a single unit of currency for
    ///     Euro is
    ///     1.00.
    /// </summary>
    private readonly bool _IsStatusCheckSupported;

    private readonly bool _IsZeroAmountAllowed;
    private readonly bool _IsZeroAmountAllowedForOffline;

    #endregion

    #region Constructor

    public TransactionProfile(
        MerchantIdentifier merchantIdentifier,
        TerminalIdentification terminalIdentification,
        InterfaceDeviceSerialNumber interfaceDeviceSerialNumber,
        CombinationCompositeKey key,
        ApplicationPriorityIndicator applicationPriorityIndicator,
        ReaderContactlessTransactionLimit readerContactlessTransactionLimit,
        ReaderCvmRequiredLimit readerCvmRequiredLimit,
        TerminalFloorLimit terminalFloorLimit,
        TerminalTransactionQualifiers terminalTransactionQualifiers,
        TerminalCategoriesSupportedList terminalCategoriesSupportedList,
        bool isStatusCheckSupported,
        bool isZeroAmountAllowed,
        bool isZeroAmountAllowedForOffline,
        bool isExtendedSelectionSupported)
    {
        _Key = key;
        _ApplicationPriorityIndicator = applicationPriorityIndicator;
        _ReaderContactlessTransactionLimit = readerContactlessTransactionLimit;
        _ReaderCvmRequiredLimit = readerCvmRequiredLimit;
        _TerminalFloorLimit = terminalFloorLimit;
        _IsStatusCheckSupported = isStatusCheckSupported;
        _IsZeroAmountAllowed = isZeroAmountAllowed;
        _IsZeroAmountAllowedForOffline = isZeroAmountAllowedForOffline;
        _IsExtendedSelectionSupported = isExtendedSelectionSupported;
        _MerchantIdentifier = merchantIdentifier;
        _TerminalIdentification = terminalIdentification;
        _InterfaceDeviceSerialNumber = interfaceDeviceSerialNumber;
        _TerminalTransactionQualifiers = terminalTransactionQualifiers;
        _TerminalCategoriesSupportedList = terminalCategoriesSupportedList;
    }

    #endregion

    #region Instance Members

    public MerchantIdentifier GetMerchantIdentifier() => _MerchantIdentifier;
    public TerminalIdentification GetTerminalIdentification() => _TerminalIdentification;
    public InterfaceDeviceSerialNumber GetInterfaceDeviceSerialNumber() => _InterfaceDeviceSerialNumber;
    public DedicatedFileName GetApplicationIdentifier() => _Key.GetApplicationId();
    public ApplicationPriorityIndicator GetApplicationPriorityIndicator() => _ApplicationPriorityIndicator;
    public ApplicationPriorityRank GetApplicationPriorityRank() => _ApplicationPriorityIndicator.GetApplicationPriorityRank();
    public ShortKernelIdTypes GetKernelId() => _Key.GetKernelId();
    public CombinationCompositeKey GetKey() => _Key;
    public ReaderContactlessTransactionLimit GetReaderContactlessTransactionLimit() => _ReaderContactlessTransactionLimit;
    public ReaderCvmRequiredLimit GetReaderCvmRequiredLimit() => _ReaderCvmRequiredLimit;
    public TerminalCategoriesSupportedList GetTerminalCategoriesSupportedList() => _TerminalCategoriesSupportedList;
    public TerminalFloorLimit GetTerminalFloorLimit() => _TerminalFloorLimit;
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