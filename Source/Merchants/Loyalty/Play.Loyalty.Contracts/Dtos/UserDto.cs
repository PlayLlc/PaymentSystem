using System.ComponentModel.DataAnnotations;

using Play.Domain;

namespace Play.Loyalty.Contracts.Dtos;

public record UserDto : IDto
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string Id { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string MerchantId { get; set; } = string.Empty;

    [Required]
    public bool IsActive { get; set; }

    #endregion
}