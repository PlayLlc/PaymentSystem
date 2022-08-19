using Play.Emv.Ber.DataElements;

namespace _TempConfiguration.Kernel;

// TODO: use KernelConfiguration PrimitiveValue, not this
public record Kernel2Configuration : KernelConfiguration
{
    #region Instance Values

    private readonly bool _IsBalanceReadingSupported;
    private readonly bool _IsDeactivateOptimizationWhenNoCdaSupported;
    private readonly bool _IsEmvModeSupported;
    private readonly bool _IsIntegratedDataStorageSupported;
    private readonly bool _IsMagstripeModeSupported;
    private readonly bool _IsOnDeviceCardholderVerificationSupported;
    private readonly bool _IsRelayResistantProtocolSupported;
    private readonly bool _IsTornTransactionRecoverySupported;

    #endregion

    #region Constructor

    // TODO: IsIntegratedDataStorageSupported is true if DSRequestedOperatorId or DSVNTerm is present in TLVDatabase with length == 0
    // TODO: IsBalanceReadingSupported is true if BalanceReadBeforeGenAC or BalanceReadAfterGenAC is present in TLVDatabase with length == 0
    public Kernel2Configuration(
        KernelConfiguration kernelConfiguration, bool isIntegratedDataStorageSupported = false, bool isBalanceReadingSupported = false,
        bool isTornTransactionRecoverySupported = false) : base(kernelConfiguration)
    {
        _IsIntegratedDataStorageSupported = false;
        _IsMagstripeModeSupported = !kernelConfiguration.IsMagstripeModeSupported();
        _IsMagstripeModeSupported = !kernelConfiguration.IsMagstripeModeSupported();
        _IsBalanceReadingSupported = false;
        _IsTornTransactionRecoverySupported = false;
        _IsOnDeviceCardholderVerificationSupported = kernelConfiguration.IsOnDeviceCardholderVerificationSupported();
        _IsRelayResistantProtocolSupported = kernelConfiguration.IsRelayResistanceProtocolSupported();
        _IsDeactivateOptimizationWhenNoCdaSupported = kernelConfiguration.IsReadAllRecordsActivated();
    }

    #endregion

    #region Instance Members

    public bool IsBalanceReadingSupported() => _IsBalanceReadingSupported;
    public bool IsDeactivateOptimizationWhenNoCdaSupported() => _IsDeactivateOptimizationWhenNoCdaSupported;
    public bool IsEmvModeOnly() => _IsEmvModeSupported && !_IsMagstripeModeSupported;
    public bool IsEmvModeSupported() => _IsEmvModeSupported;
    public bool IsIntegratedDataStorageSupported() => _IsIntegratedDataStorageSupported;
    public bool IsMagstripeModeOnlySupported() => !_IsEmvModeSupported && _IsMagstripeModeSupported;
    public bool IsMagstripeModeSupported() => _IsMagstripeModeSupported;
    public bool IsOnDeviceCardholderVerificationSupported() => _IsOnDeviceCardholderVerificationSupported;
    public bool IsRelayResistantProtocolSupported() => _IsRelayResistantProtocolSupported;
    public bool IsTornTransactionRecoverySupported() => _IsTornTransactionRecoverySupported;

    #endregion
}