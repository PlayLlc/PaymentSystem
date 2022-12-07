using Play.Ber.Exceptions;
using Play.Codecs;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Identifiers;
using Play.Emv.Selection.Configuration;
using Play.Emv.Selection.Contracts;

namespace MockPos.Dtos;

public class TransactionProfileDto
{
    #region Instance Values

    public byte KernelId { get; set; }
    public string? ApplicationId { get; set; }
    public byte TransactionType { get; set; }
    public byte ApplicationPriorityIndicator { get; set; }
    public ulong ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice { get; set; }
    public ulong ReaderContactlessTransactionLimitWhenCvmIsOnDevice { get; set; }
    public ulong ReaderContactlessFloorLimit { get; set; }
    public ulong ReaderCvmRequiredLimit { get; set; }
    public byte KernelConfiguration { get; set; }
    public uint TerminalTransactionQualifiers { get; set; }
    public bool IsStatusCheckSupported { get; set; }
    public bool IsZeroAmountAllowed { get; set; }
    public bool IsZeroAmountAllowedForOffline { get; set; }
    public bool IsExtendedSelectionSupported { get; set; }

    #endregion

    #region Serialization

    /// <exception cref="BerParsingException"></exception>
    public TransactionProfile Decode()
    {
        KernelId kernelId = new(KernelId);
        ApplicationIdentifier applicationId = ApplicationIdentifier.Decode(PlayCodec.HexadecimalCodec.Encode(ApplicationId).AsSpan());
        TransactionType transactionType = new(TransactionType);
        ApplicationPriorityIndicator applicationPriorityIndicator = new(ApplicationPriorityIndicator);

        // BUG: We should have both of the derived ReaderContactlessTransactionLimit types in the TransactionProfileDto
        ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice readerContactlessTransactionLimitWhenCvmIsNotOnDevice =
            new(ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice);
        ReaderContactlessTransactionLimitWhenCvmIsOnDevice readerContactlessTransactionLimitWhenCvmIsOnDevice =
            new(ReaderContactlessTransactionLimitWhenCvmIsOnDevice);
        ReaderContactlessFloorLimit readerContactlessFloorLimit = new(ReaderContactlessFloorLimit);
        ReaderCvmRequiredLimit readerCvmRequiredLimit = new(ReaderCvmRequiredLimit);
        KernelConfiguration kernelConfiguration = new(KernelConfiguration);
        TerminalTransactionQualifiers terminalTransactionQualifiers = new(TerminalTransactionQualifiers);
        CombinationCompositeKey combinationCompositeKey = new(applicationId, kernelId, transactionType);

        return new TransactionProfile(combinationCompositeKey, applicationPriorityIndicator, readerContactlessTransactionLimitWhenCvmIsNotOnDevice,
            readerContactlessTransactionLimitWhenCvmIsOnDevice, readerContactlessFloorLimit, readerCvmRequiredLimit, terminalTransactionQualifiers,
            kernelConfiguration, IsStatusCheckSupported, IsZeroAmountAllowed, IsZeroAmountAllowedForOffline, IsExtendedSelectionSupported);
    }

    #endregion
}