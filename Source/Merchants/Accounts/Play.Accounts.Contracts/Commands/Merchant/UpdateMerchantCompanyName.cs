using System.ComponentModel.DataAnnotations;

namespace Play.Accounts.Contracts.Commands;

public record UpdateMerchantCompanyName
{
    #region Instance Values

    [Required]
    [MinLength(1)]
    public string Id { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public string CompanyName { get; set; } = string.Empty;

    #endregion
}