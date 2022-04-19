using Play.Core.Extensions;

namespace Play.Emv.Ber;

/// <summary>
///     An intermediate value returned from a section of processing
/// </summary>
public readonly record struct TerminalVerificationResult
{
    #region Instance Values

    private readonly ulong _Value;

    #endregion

    #region Constructor

    public TerminalVerificationResult(ulong value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public static TerminalVerificationResult Create() => new(0);
    public void SetApplicationNotYetEffective() => _Value.SetBit(30);
    public void SetCardAppearsOnTerminalExceptionFile() => _Value.SetBit(37);
    public void SetCardholderVerificationWasNotSuccessful() => _Value.SetBit(24);
    public void SetCombinationDataAuthenticationFailed() => _Value.SetBit(35);
    public void SetDefaultTransactionCertificateDataObjectListUsed() => _Value.SetBit(8);
    public void SetDynamicDataAuthenticationFailed() => _Value.SetBit(36);
    public void SetExpiredApplication() => _Value.SetBit(31);
    public void SetIccAndTerminalHaveDifferentApplicationVersions() => _Value.SetBit(32);
    public void SetIccDataMissing() => _Value.SetBit(38);
    public void SetIssuerAuthenticationFailed() => _Value.SetBit(7);
    public void SetLowerConsecutiveOfflineLimitExceeded() => _Value.SetBit(15);
    public void SetMerchantForcedTransactionOnline() => _Value.SetBit(12);
    public void SetNewCard() => _Value.SetBit(28);
    public void SetOfflineDataAuthenticationWasNotPerformed() => _Value.SetBit(40);
    public void SetOnlinePinEntered() => _Value.SetBit(19);
    public void SetPinEntryRequiredAndPinPadNotPresentOrNotWorking() => _Value.SetBit(21);
    public void SetPinEntryRequiredPinPadPresentButPinWasNotEntered() => _Value.SetBit(20);
    public void SetPinTryLimitExceeded() => _Value.SetBit(22);
    public void SetRelayResistanceThresholdExceeded() => _Value.SetBit(4);
    public void SetRelayResistanceTimeLimitsExceeded() => _Value.SetBit(3);
    public void SetRequestedServiceNotAllowedForCardProduct() => _Value.SetBit(29);
    public void SetScriptProcessingFailedAfterFinalGenerateAc() => _Value.SetBit(5);
    public void SetScriptProcessingFailedBeforeFinalGenerateAc() => _Value.SetBit(6);
    public void SetStaticDataAuthenticationFailed() => _Value.SetBit(39);
    public void SetTransactionExceedsFloorLimit() => _Value.SetBit(16);
    public void SetTransactionSelectedRandomlyForOnlineProcessing() => _Value.SetBit(13);
    public void SetUnrecognizedCvm() => _Value.SetBit(23);
    public void SetUpperConsecutiveOfflineLimitExceeded() => _Value.SetBit(14);
    public bool CombinationDataAuthenticationFailed() => _Value.IsBitSet(35);

    #endregion

    #region Operator Overrides

    public static explicit operator ulong(TerminalVerificationResult value) => value._Value;

    #endregion
}