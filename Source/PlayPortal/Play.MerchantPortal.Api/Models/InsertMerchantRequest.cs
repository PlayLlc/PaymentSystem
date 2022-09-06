using System.ComponentModel.DataAnnotations;

namespace Play.MerchantPortal.Api.Models;

public class InsertMerchantRequest : MerchantBaseRequest
{
    #region Instance Values

    [Required]
    public long CompanyId { get; set; }

    #endregion
}