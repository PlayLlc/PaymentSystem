using Play.Core;
using Play.Core.Extensions;

namespace Play.Emv.DataElements.Emv;

public record TerminalVerificationResultCodes : EnumObject<TerminalVerificationResult>
{
    #region Static Metadata

    public static TerminalVerificationResultCodes ApplicationNotYetEffective;
    public static TerminalVerificationResultCodes CardAppearsOnTerminalExceptionFile;
    public static TerminalVerificationResultCodes CardholderVerificationWasNotSuccessful;
    public static TerminalVerificationResultCodes CombinationDataAuthenticationFailed;
    public static TerminalVerificationResultCodes DefaultTransactionCertificateDataObjectListUsed;
    public static TerminalVerificationResultCodes DynamicDataAuthenticationFailed;
    public static TerminalVerificationResultCodes ExpiredApplication;
    public static TerminalVerificationResultCodes IccAndTerminalHaveDifferentApplicationVersions;
    public static TerminalVerificationResultCodes IccDataMissing;
    public static TerminalVerificationResultCodes IssuerAuthenticationFailed;
    public static TerminalVerificationResultCodes LowerConsecutiveOfflineLimitExceeded;
    public static TerminalVerificationResultCodes MerchantForcedTransactionOnline;
    public static TerminalVerificationResultCodes NewCard;
    public static TerminalVerificationResultCodes OfflineDataAuthenticationWasNotPerformed;
    public static TerminalVerificationResultCodes OnlinePinEntered;
    public static TerminalVerificationResultCodes PinEntryRequiredAndPinPadNotPresentOrNotWorking;
    public static TerminalVerificationResultCodes PinEntryRequiredPinPadPresentButPinWasNotEntered;
    public static TerminalVerificationResultCodes PinTryLimitExceeded;
    public static TerminalVerificationResultCodes RelayResistanceThresholdExceeded;
    public static TerminalVerificationResultCodes RelayResistanceTimeLimitsExceeded;
    public static TerminalVerificationResultCodes RequestedServiceNotAllowedForCardProduct;
    public static TerminalVerificationResultCodes ScriptProcessingFailedAfterFinalGenerateAc;
    public static TerminalVerificationResultCodes ScriptProcessingFailedBeforeFinalGenerateAc;
    public static TerminalVerificationResultCodes StaticDataAuthenticationFailed;
    public static TerminalVerificationResultCodes TransactionExceedsFloorLimit;
    public static TerminalVerificationResultCodes TransactionSelectedRandomlyForOnlineProcessing;
    public static TerminalVerificationResultCodes UnrecognizedCvm;
    public static TerminalVerificationResultCodes UpperConsecutiveOfflineLimitExceeded;

    #endregion

    #region Constructor

    static TerminalVerificationResultCodes()
    {
        ulong a = ((ulong) 0).SetBit(30);
        ApplicationNotYetEffective = new TerminalVerificationResultCodes(new TerminalVerificationResult(((ulong) 0).SetBit(30)));
        CardAppearsOnTerminalExceptionFile = new TerminalVerificationResultCodes(new TerminalVerificationResult(((ulong) 0).SetBit(37)));
        CardholderVerificationWasNotSuccessful =
            new TerminalVerificationResultCodes(new TerminalVerificationResult(((ulong) 0).SetBit(24)));
        CombinationDataAuthenticationFailed = new TerminalVerificationResultCodes(new TerminalVerificationResult(((ulong) 0).SetBit(35)));
        DefaultTransactionCertificateDataObjectListUsed =
            new TerminalVerificationResultCodes(new TerminalVerificationResult(((ulong) 0).SetBit(8)));
        DynamicDataAuthenticationFailed = new TerminalVerificationResultCodes(new TerminalVerificationResult(((ulong) 0).SetBit(36)));
        ExpiredApplication = new TerminalVerificationResultCodes(new TerminalVerificationResult(((ulong) 0).SetBit(31)));
        IccAndTerminalHaveDifferentApplicationVersions =
            new TerminalVerificationResultCodes(new TerminalVerificationResult(((ulong) 0).SetBit(32)));
        IccDataMissing = new TerminalVerificationResultCodes(new TerminalVerificationResult(((ulong) 0).SetBit(38)));
        IssuerAuthenticationFailed = new TerminalVerificationResultCodes(new TerminalVerificationResult(((ulong) 0).SetBit(7)));
        LowerConsecutiveOfflineLimitExceeded = new TerminalVerificationResultCodes(new TerminalVerificationResult(((ulong) 0).SetBit(15)));
        MerchantForcedTransactionOnline = new TerminalVerificationResultCodes(new TerminalVerificationResult(((ulong) 0).SetBit(12)));
        NewCard = new TerminalVerificationResultCodes(new TerminalVerificationResult(((ulong) 0).SetBit(28)));
        OfflineDataAuthenticationWasNotPerformed =
            new TerminalVerificationResultCodes(new TerminalVerificationResult(((ulong) 0).SetBit(40)));
        OnlinePinEntered = new TerminalVerificationResultCodes(new TerminalVerificationResult(((ulong) 0).SetBit(19)));
        PinEntryRequiredAndPinPadNotPresentOrNotWorking =
            new TerminalVerificationResultCodes(new TerminalVerificationResult(((ulong) 0).SetBit(21)));
        PinEntryRequiredPinPadPresentButPinWasNotEntered =
            new TerminalVerificationResultCodes(new TerminalVerificationResult(((ulong) 0).SetBit(20)));
        PinTryLimitExceeded = new TerminalVerificationResultCodes(new TerminalVerificationResult(((ulong) 0).SetBit(22)));
        RelayResistanceThresholdExceeded = new TerminalVerificationResultCodes(new TerminalVerificationResult(((ulong) 0).SetBit(4)));
        RelayResistanceTimeLimitsExceeded = new TerminalVerificationResultCodes(new TerminalVerificationResult(((ulong) 0).SetBit(3)));
        RequestedServiceNotAllowedForCardProduct =
            new TerminalVerificationResultCodes(new TerminalVerificationResult(((ulong) 0).SetBit(29)));
        ScriptProcessingFailedAfterFinalGenerateAc =
            new TerminalVerificationResultCodes(new TerminalVerificationResult(((ulong) 0).SetBit(5)));
        ScriptProcessingFailedBeforeFinalGenerateAc =
            new TerminalVerificationResultCodes(new TerminalVerificationResult(((ulong) 0).SetBit(6)));
        StaticDataAuthenticationFailed = new TerminalVerificationResultCodes(new TerminalVerificationResult(((ulong) 0).SetBit(39)));
        TransactionExceedsFloorLimit = new TerminalVerificationResultCodes(new TerminalVerificationResult(((ulong) 0).SetBit(16)));
        TransactionSelectedRandomlyForOnlineProcessing =
            new TerminalVerificationResultCodes(new TerminalVerificationResult(((ulong) 0).SetBit(13)));
        UnrecognizedCvm = new TerminalVerificationResultCodes(new TerminalVerificationResult(((ulong) 0).SetBit(23)));
        UpperConsecutiveOfflineLimitExceeded = new TerminalVerificationResultCodes(new TerminalVerificationResult(((ulong) 0).SetBit(14)));
    }

    private TerminalVerificationResultCodes(TerminalVerificationResult value) : base(value)
    { }

    #endregion
}