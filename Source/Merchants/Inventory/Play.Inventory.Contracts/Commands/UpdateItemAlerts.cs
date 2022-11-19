using System.ComponentModel.DataAnnotations;

namespace Play.Inventory.Contracts.Commands;

public record UpdateItemAlerts
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string UserId { get; set; } = string.Empty;

    #endregion
}