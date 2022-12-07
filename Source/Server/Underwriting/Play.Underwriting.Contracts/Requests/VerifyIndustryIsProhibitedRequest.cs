using System.ComponentModel.DataAnnotations;

namespace Play.Underwriting.Contracts.Requests;

public record VerifyIndustryIsProhibitedRequest
{
    #region Instance Values

    [Required]
    public ushort MerchantCategoryCode { get; set; }

    #endregion
}