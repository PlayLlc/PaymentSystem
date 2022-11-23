using System.ComponentModel.DataAnnotations;

namespace Play.Loyalty.Contracts.Commands;

public record RemoveDiscountedItemPrice
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string DiscountId { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string UserId { get; set; } = string.Empty;

    #endregion
}