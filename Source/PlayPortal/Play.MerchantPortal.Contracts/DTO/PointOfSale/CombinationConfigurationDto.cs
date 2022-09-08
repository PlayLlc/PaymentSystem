namespace Play.MerchantPortal.Contracts.DTO.PointOfSale;

public class CombinationConfigurationDto
{
    public int KernelId { get; set; }

    public string ApplicationId { get; set; } = string.Empty;

    public byte TransactionType { get; set; }

    public byte ApplicationPriorityIndicator { get; set; }

    public ulong ContactlessTransactionLimit { get; set; }

    public ulong ContactlessFloorLimit { get; set; }

    public ulong CvmRequiredLimit { get; set; }

    public byte KernelConfiguration { get; set; }

    public uint TerminalTransactionQualifiers { get; set; }

    public bool IsStatusCheckSupported { get; set; }

    public bool IsZeroAmountAllowed { get; set; }

    public bool IsZeroAmountAllowedForOffline { get; set; }

    public bool IsExtendedSelectionSupported { get; set; }
}
