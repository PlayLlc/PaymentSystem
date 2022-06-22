using System;

using Play.Codecs.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Kernel.Databases;

public partial class KernelDatabase
{
    #region Configuration

    /// <exception cref="TerminalDataException"></exception>
    public ReaderContactlessTransactionLimit GetReaderContactlessTransactionLimit()
    {
        if (IsOnDeviceCardholderVerificationSupported())
            return (ReaderContactlessTransactionLimitWhenCvmIsOnDevice) Get(ReaderContactlessTransactionLimitWhenCvmIsOnDevice.Tag);

        return (ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice) Get(ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice.Tag);
    }

    #region Get Configuration Objects

    /// <exception cref="TerminalDataException"></exception>
    private TerminalCapabilities GetTerminalCapabilities() => Get<TerminalCapabilities>(TerminalCapabilities.Tag);

    /// <exception cref="TerminalDataException"></exception>
    private TransactionType GetTransactionType() => (TransactionType) Get(TransactionType.Tag);

    /// <exception cref="TerminalDataException"></exception>
    private IntegratedDataStorageStatus GetIntegratedDataStorageStatus() => Get<IntegratedDataStorageStatus>(IntegratedDataStorageStatus.Tag);

    /// <exception cref="TerminalDataException"></exception>
    private TerminalType GetTerminalType() => Get<TerminalType>(TerminalType.Tag);

    /// <exception cref="TerminalDataException"></exception>
    private PosEntryMode GetPosEntryMode() => Get<PosEntryMode>(PosEntryMode.Tag);

    #endregion

    #region Terminal Type

    /// <exception cref="TerminalDataException"></exception>
    public bool IsTerminalType(TerminalType.EnvironmentType environment) => GetTerminalType().IsEnvironmentType(environment);

    /// <exception cref="TerminalDataException"></exception>
    public bool IsTerminalType(TerminalType.CommunicationType communicationType) => GetTerminalType().IsCommunicationType(communicationType);

    /// <exception cref="TerminalDataException"></exception>
    public bool IsTerminalType(TerminalType.TerminalOperatorType operatorType) => GetTerminalType().IsOperatorType(operatorType);

    #endregion

    #region Terminal Capabilities

    #region Read

    /// <exception cref="TerminalDataException"></exception>
    public bool IsCardCaptureSupported() => GetTerminalCapabilities().IsCardCaptureSupported();

    /// <exception cref="TerminalDataException"></exception>
    public bool IsCombinedDataAuthenticationSupported() => GetTerminalCapabilities().IsCombinedDataAuthenticationSupported();

    /// <exception cref="TerminalDataException"></exception>
    public bool IsDynamicDataAuthenticationSupported() => GetTerminalCapabilities().IsDynamicDataAuthenticationSupported();

    /// <exception cref="TerminalDataException"></exception>
    public bool IsEncipheredPinForOfflineVerificationSupported() => GetTerminalCapabilities().IsEncipheredPinForOfflineVerificationSupported();

    /// <exception cref="TerminalDataException"></exception>
    public bool IsEncipheredPinForOnlineVerificationSupported() => GetTerminalCapabilities().IsEncipheredPinForOnlineVerificationSupported();

    /// <exception cref="TerminalDataException"></exception>
    public bool IsIcWithContactsSupported() => GetTerminalCapabilities().IsIcWithContactsSupported();

    /// <exception cref="TerminalDataException"></exception>
    public bool IsMagneticStripeSupported() => GetTerminalCapabilities().IsMagneticStripeSupported();

    /// <exception cref="TerminalDataException"></exception>
    public bool IsManualKeyEntrySupported() => GetTerminalCapabilities().IsManualKeyEntrySupported();

    /// <exception cref="TerminalDataException"></exception>
    public bool IsNoCardVerificationMethodRequiredSupported() => GetTerminalCapabilities().IsNoCardVerificationMethodRequiredSet();

    /// <exception cref="TerminalDataException"></exception>
    public bool IsPlaintextPinForIccVerificationSupported() => GetTerminalCapabilities().IsCardCaptureSupported();

    /// <exception cref="TerminalDataException"></exception>
    public bool IsSignaturePaperSupported() => GetTerminalCapabilities().IsSignaturePaperSupported();

    /// <exception cref="TerminalDataException"></exception>
    public bool IsStaticDataAuthenticationSupported() => GetTerminalCapabilities().IsStaticDataAuthenticationSupported();

    #endregion

    #region Write

    /// <exception cref="TerminalDataException"></exception>
    public void SetCardVerificationMethodNotRequired(bool value)
    {
        try
        {
            _TerminalCapabilitiesBuilder.Reset(GetTerminalCapabilities());
            _TerminalCapabilitiesBuilder.SetCardVerificationMethodNotRequired(value);
            Update(_TerminalCapabilitiesBuilder.Complete());
        }
        catch (DataElementParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
        }
        catch (CodecParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
        }
        catch (Exception exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
        }
    }

    #endregion

    #endregion

    #region Data Storage Flags

    /// <exception cref="TerminalDataException"></exception>
    public bool IsIntegratedDataStorageReadFlagSet() => GetIntegratedDataStorageStatus().IsReadSet();

    /// <exception cref="TerminalDataException"></exception>
    public bool IsIntegratedDataStorageWriteFlagSet() => GetIntegratedDataStorageStatus().IsWriteSet();

    /// <summary>
    ///     Indicates if this payment system supports Integrated Data Storage and Torn Transaction Recovery
    /// </summary>
    /// <returns></returns>
    /// <exception cref="TerminalDataException"></exception>
    public bool IsIdsAndTtrImplemented() => IsIntegratedDataStorageSupported() && IsTornTransactionRecoverySupported();

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

    #endregion

    #region Kernel Configuration

    /// <exception cref="TerminalDataException"></exception>
    public bool IsEmvModeSupported() => GetKernelConfiguration().IsEmvModeSupported();

    /// <exception cref="TerminalDataException"></exception>
    public bool IsMagstripeModeSupported() => GetKernelConfiguration().IsMagstripeModeSupported();

    /// <exception cref="TerminalDataException"></exception>
    public bool IsOnDeviceCardholderVerificationSupported() => GetKernelConfiguration().IsOnDeviceCardholderVerificationSupported();

    /// <exception cref="TerminalDataException"></exception>
    public bool IsRelayResistanceProtocolSupported() => GetKernelConfiguration().IsRelayResistanceProtocolSupported();

    /// <exception cref="TerminalDataException"></exception>
    public bool IsReadAllRecordsActivated() => GetKernelConfiguration().IsReadAllRecordsActivated();

    #endregion

    #endregion
}