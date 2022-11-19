using System.ComponentModel.DataAnnotations;

namespace Play.Underwriting.Contracts.Requests;

public record VerifyIndustryIsProhibitedRequest
{
    [Required]
    public ushort MerchantCategoryCode { get; set; }
}
