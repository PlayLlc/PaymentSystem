using System.ComponentModel.DataAnnotations;

using Play.Domain;

namespace Play.Identity.Contracts.Dtos;

public record BusinessInfoDto : IDto
{
    #region Instance Values

    /// <summary>
    ///     The Identifier of this Business Info entity
    /// </summary>
    [Required]
    [MinLength(1)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    ///     The business type of the Merchant
    /// </summary>
    [Required]
    [MinLength(1)]
    public string BusinessType { get; set; } = string.Empty;

    /// <summary>
    ///     The Merchant Category Code associated with the Merchant
    /// </summary>
    [Required]
    [Range(1, 9999)]
    public ushort MerchantCategoryCode { get; set; }

    #endregion
}