using System.ComponentModel.DataAnnotations;

namespace Play.Loyalty.Contracts.Commands;

public record CreateLoyaltyProgram
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string MerchantId { get; set; }

    [Required]
    [StringLength(20)]
    public string UserId { get; set; }

    #endregion
}