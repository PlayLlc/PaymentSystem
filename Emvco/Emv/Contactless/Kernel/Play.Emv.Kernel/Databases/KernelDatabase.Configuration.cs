using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Kernel.Databases;

public abstract partial class KernelDatabase
{
    #region Configuration

    public ReaderContactlessTransactionLimit GetReaderContactlessTransactionLimit()
    {
        if (IsOnDeviceCardholderVerificationSupported())
            return (ReaderContactlessTransactionLimitWhenCvmIsOnDevice) Get(ReaderContactlessTransactionLimitWhenCvmIsOnDevice.Tag);

        return (ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice) Get(ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice.Tag);
    }

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    private TerminalCapabilities GetTerminalCapabilities() => (TerminalCapabilities) Get(TerminalCapabilities.Tag);

    private TransactionType GetTransactionType() => (TransactionType) Get(TransactionType.Tag);
    private TerminalType GetTerminalType() => (TerminalType) Get(TerminalType.Tag);
    private PosEntryMode GetPosEntryMode() => (PosEntryMode) Get(PosEntryMode.Tag);
    public bool IsCardCaptureSupported() => GetTerminalCapabilities().IsCardCaptureSupported();
    public bool IsCombinedDataAuthenticationSupported() => GetTerminalCapabilities().IsCombinedDataAuthenticationSupported();
    public bool IsDynamicDataAuthenticationSupported() => GetTerminalCapabilities().IsDynamicDataAuthenticationSupported();

    public bool IsEncipheredPinForOfflineVerificationSupported() =>
        GetTerminalCapabilities().IsEncipheredPinForOfflineVerificationSupported();

    public bool IsEncipheredPinForOnlineVerificationSupported() =>
        GetTerminalCapabilities().IsEncipheredPinForOnlineVerificationSupported();

    public bool IsIcWithContactsSupported() => GetTerminalCapabilities().IsIcWithContactsSupported();
    public bool IsMagneticStripeSupported() => GetTerminalCapabilities().IsMagneticStripeSupported();
    public bool IsManualKeyEntrySupported() => GetTerminalCapabilities().IsManualKeyEntrySupported();
    public bool IsNoCardVerificationMethodRequiredSupported() => GetTerminalCapabilities().IsNoCardVerificationMethodRequiredSupported();
    public bool IsPlaintextPinForIccVerificationSupported() => GetTerminalCapabilities().IsCardCaptureSupported();
    public bool IsSignaturePaperSupported() => GetTerminalCapabilities().IsSignaturePaperSupported();
    public bool IsStaticDataAuthenticationSupported() => GetTerminalCapabilities().IsStaticDataAuthenticationSupported();

    /// <summary>
    ///     IDS builds the reading and writing functions into existing payment commands (GET PROCESSING OPTIONS and GENERATE
    ///     AC). The command-response sequence exchanged between the Card and Kernel is therefore unchanged from a normal
    ///     purchase transaction. It also addresses the security mechanisms of those exchanges.
    /// </summary>
    /// <returns></returns>
    /// <remarks>EMV Book C-2 Section 3.3</remarks>
    /// <exception cref="TerminalDataException"></exception>
    public bool IsIntegratedDataStorageSupported() =>
        IsPresent(DataStorageRequestedOperatorId.Tag) && IsPresentAndNotEmpty(DataStorageVersionNumberTerminal.Tag);

    /// <summary>
    ///     Through the number of entries possible in the torn transaction log, indicated by the value of data object Max
    ///     Number of Torn Transaction Log Records If Max Number of Torn Transaction Log Records is present and set to a value
    ///     different from zero, then torn transaction recovery is supported.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="TerminalDataException"></exception>
    public bool IsTornTransactionRecoverySupported() =>
        IsPresentAndNotEmpty(MaxNumberOfTornTransactionLogRecords.Tag)
        && (((MaxNumberOfTornTransactionLogRecords) Get(MaxNumberOfTornTransactionLogRecords.Tag)).GetValueByteCount() > 0);

    /// <summary>
    ///     Indicates if this payment system supports Integrated Data Storage and Torn Transaction Recovery
    /// </summary>
    /// <returns></returns>
    /// <exception cref="TerminalDataException"></exception>
    public bool IsIdsAndTtrSupported() => IsIntegratedDataStorageSupported() && IsTornTransactionRecoverySupported();

    public bool IsEmvModeSupported() => GetKernelConfiguration().IsEmvModeSupported();
    public bool IsMagstripeModeSupported() => GetKernelConfiguration().IsMagstripeModeSupported();
    public bool IsOnDeviceCardholderVerificationSupported() => GetKernelConfiguration().IsOnDeviceCardholderVerificationSupported();
    public bool IsRelayResistanceProtocolSupported() => GetKernelConfiguration().IsRelayResistanceProtocolSupported();
    public bool IsReadAllRecordsActivated() => GetKernelConfiguration().IsReadAllRecordsActivated();

    #endregion
}