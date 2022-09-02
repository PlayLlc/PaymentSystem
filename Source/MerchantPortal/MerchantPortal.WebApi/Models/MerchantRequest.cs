using System.ComponentModel.DataAnnotations;

namespace MerchantPortal.WebApi.Models;

public class MerchantBaseRequest
{
    public string? AcquirerId { get; set; }

    public string? MerchantIdentifier { get; set; }

    public short MerchantCategoryCode { get; set; }

    public string? Name { get; set; }

    public string? StreetAddress { get; set; }

    public string? City { get; set; }

    public string? ZipCode { get; set; }

    public string? State { get; set; }

    public string? Country { get; set; }
}

public class InsertMerchantRequest : MerchantBaseRequest
{
    [Required]
    public long CompanyId { get; set; }
}

public class UpdateMerchantRequest : MerchantBaseRequest { }
