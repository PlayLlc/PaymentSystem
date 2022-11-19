using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Dtos;

namespace Play.Underwriting.Contracts.Requests;

public record VerifyMerchantIsProhibitedRequest
{
    #region Instance Values

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public AddressDto? Address { get; set; }

    #endregion
}