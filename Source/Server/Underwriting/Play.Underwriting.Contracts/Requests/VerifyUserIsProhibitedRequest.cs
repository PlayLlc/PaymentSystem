using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Dtos;

namespace Play.Underwriting.Contracts.Requests;

public record VerifyUserIsProhibitedRequest
{
    #region Instance Values

    [Required]
    public AddressDto? Address { get; set; }

    [Required]
    public ContactDto? Contact { get; set; }

    #endregion
}