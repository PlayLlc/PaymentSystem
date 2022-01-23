using Play.Emv.DataElements;
using Play.Icc.FileSystem.DedicatedFiles;

namespace Play.Emv.Configuration;

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
    private readonly TerminalFloorLimit _TerminalFloorLimit;
    private readonly TerminalTransactionQualifiers _TerminalTransactionQualifiers;

    /// <summary>
    ///     Option within the terminal related to the checking of a single unit of currency.A single unit of currency has the
    ///     value of
    ///     1 of the(major) unit of currency as defined in [ISO 4217]. As an example a single unit of currency for Euro is
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

    public MerchantIdentifier GetMerchantIdentifier()
    {
        return _MerchantIdentifier;
    }

    public TerminalIdentification GetTerminalIdentification()
    {
        return _TerminalIdentification;
    }

    public InterfaceDeviceSerialNumber GetInterfaceDeviceSerialNumber()
    {
        return _InterfaceDeviceSerialNumber;
    }

    public DedicatedFileName GetApplicationIdentifier()
    {
        return _Key.GetApplicationId();
    }

    public ApplicationPriorityIndicator GetApplicationPriorityIndicator()
    {
        return _ApplicationPriorityIndicator;
    }

    public ApplicationPriorityRank GetApplicationPriorityRank()
    {
        return _ApplicationPriorityIndicator.GetApplicationPriorityRank();
    }

    public ShortKernelId GetKernelId()
    {
        return _Key.GetKernelId();
    }

    public CombinationCompositeKey GetKey()
    {
        return _Key;
    }

    public ReaderContactlessTransactionLimit GetReaderContactlessTransactionLimit()
    {
        return _ReaderContactlessTransactionLimit;
    }

    public ReaderCvmRequiredLimit GetReaderCvmRequiredLimit()
    {
        return _ReaderCvmRequiredLimit;
    }

    public TerminalCategoriesSupportedList GetTerminalCategoriesSupportedList()
    {
        return _TerminalCategoriesSupportedList;
    }

    public TerminalFloorLimit GetTerminalFloorLimit()
    {
        return _TerminalFloorLimit;
    }

    public TerminalTransactionQualifiers GetTerminalTransactionQualifiers()
    {
        return _TerminalTransactionQualifiers;
    }

    public TransactionType GetTransactionType()
    {
        return _Key.GetTransactionType();
    }

    public bool IsExtendedSelectionSupported()
    {
        return _IsExtendedSelectionSupported;
    }

    public bool IsStatusCheckSupported()
    {
        return _IsStatusCheckSupported;
    }

    public bool IsZeroAmountAllowed()
    {
        return _IsZeroAmountAllowed;
    }

    public bool IsZeroAmountAllowedForOffline()
    {
        return _IsZeroAmountAllowedForOffline;
    }

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

    public override bool Equals(object? obj)
    {
        return obj is TransactionProfile transactionProfile && Equals(transactionProfile);
    }

    public int GetHashCode(TransactionProfile obj)
    {
        return obj.GetHashCode();
    }

    public override int GetHashCode()
    {
        return _Key.GetHashCode();
    }

    #endregion

    //public PoiInformation GetPoiInformation(BerCodec codec) =>
}