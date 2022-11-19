using System.ComponentModel.DataAnnotations;

namespace Play.Inventory.Contracts.Commands;

public record CreateCategory
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string MerchantId { get; set; }

    [Required]
    [StringLength(20)]
    public string UserId { get; set; }

    [Required]
    [MinLength(1)]
    public string Name { get; set; }

    #endregion
}