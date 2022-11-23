using System.ComponentModel.DataAnnotations;

using Play.Inventory.Contracts.Dtos;

namespace Play.Loyalty.Contracts.Commands;

public record UpdateRewardPoints
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string MerchantId { get; set; } = string.Empty;

    [Required]
    public MoneyDto TransactionAmount { get; set; } = null!;

    #endregion
}