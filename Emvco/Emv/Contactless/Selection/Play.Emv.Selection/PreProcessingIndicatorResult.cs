using Play.Emv.Ber.DataElements;
using Play.Emv.DataElements;
using Play.Emv.Identifiers;
using Play.Icc.FileSystem.DedicatedFiles;

namespace Play.Emv.Selection;

/// <summary>
///     The value object created during Entry Point Processing. This result is used as part of the Kernel
///     configuration
/// </summary>
public class PreProcessingIndicatorResult
{
    #region Instance Values

    private readonly CombinationCompositeKey _CombinationCompositeKey;
    private readonly TerminalTransactionQualifiers _TerminalTransactionQualifiers;

    #endregion

    #region Constructor

    public PreProcessingIndicatorResult(
        CombinationCompositeKey combinationCompositeKey,
        TerminalTransactionQualifiers terminalTransactionQualifiers)
    {
        _CombinationCompositeKey = combinationCompositeKey;
        _TerminalTransactionQualifiers = terminalTransactionQualifiers;
    }

    #endregion

    #region Instance Members

    public TerminalTransactionQualifiers GetTerminalTransactionQualifiers() => _TerminalTransactionQualifiers;
    public DedicatedFileName GetApplicationIdentifier() => _CombinationCompositeKey.GetApplicationId();
    public CombinationCompositeKey GetCombinationCompositeKey() => _CombinationCompositeKey;
    public KernelId GetKernelId() => _CombinationCompositeKey.GetKernelId();
    public TransactionType GetTransactionType() => _CombinationCompositeKey.GetTransactionType();
    public bool IsCvmRequired() => _TerminalTransactionQualifiers.IsCvmRequired();
    public bool IsCvmSupportedOnConsumerDevice() => _TerminalTransactionQualifiers.IsCvmSupportedOnConsumerDevice();
    public bool IsEmvContactSupported() => _TerminalTransactionQualifiers.IsEmvContactSupported();
    public bool IsEmvModeSupported() => _TerminalTransactionQualifiers.IsEmvModeSupported();
    public bool IsIssuerUpdateProcessingSupported() => _TerminalTransactionQualifiers.IsIssuerUpdateProcessingSupported();
    public bool IsMagStripeModeSupported() => _TerminalTransactionQualifiers.IsMagStripeModeSupported();

    public bool IsOfflineDataAuthenticationForOnlineAuthorizationsSupported() =>
        _TerminalTransactionQualifiers.IsOfflineDataAuthenticationForOnlineAuthorizationsSupported();

    public bool IsOfflinePinSupportedForEmvContact() => _TerminalTransactionQualifiers.IsOfflinePinSupportedForEmvContact();
    public bool IsOnlineCryptogramRequired() => _TerminalTransactionQualifiers.IsOnlineCryptogramRequired();
    public bool IsOnlinePinSupported() => _TerminalTransactionQualifiers.IsOnlinePinSupported();
    public bool IsReaderOfflineOnly() => _TerminalTransactionQualifiers.IsReaderOfflineOnly();
    public bool IsReaderOnlineCapable() => _TerminalTransactionQualifiers.IsReaderOnlineCapable();
    public bool IsSignatureSupported() => _TerminalTransactionQualifiers.IsSignatureSupported();

    #endregion
}