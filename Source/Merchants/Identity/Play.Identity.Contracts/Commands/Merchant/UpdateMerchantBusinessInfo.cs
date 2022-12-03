using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Attributes;
using Play.Identity.Contracts.Dtos;

namespace Play.Identity.Contracts.Commands;

public record UpdateMerchantBusinessInfo
{
    #region Instance Values

    [Required]
    [AlphaNumericSpecial]
    [StringLength(20)]
    public string Id { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public BusinessInfoDto BusinessInfo { get; set; } = new BusinessInfoDto();

    #endregion
}