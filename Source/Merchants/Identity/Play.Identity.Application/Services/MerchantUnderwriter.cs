using Microsoft.Extensions.Options;

using System.Text.Json;

using Play.Domain.Common.Entities;
using Play.Domain.Common.ValueObjects;
using Play.Identity.Domain.Entities;
using Play.Identity.Domain.Services;
using Play.Identity.Domain.ValueObjects;

namespace Play.Identity.Application.Services;

public class MerchantUnderwriter : IUnderwriteMerchants
{
    #region Instance Values

    private readonly HttpClient _Client;
    private readonly JsonSerializerOptions _Options;

    #endregion

    #region Constructor

    public MerchantUnderwriter(HttpClient client, IOptions<JsonSerializerOptions> options)
    {
        _Client = client;
        _Options = options.Value;
    }

    #endregion

    #region Instance Members

    public async Task<bool> IsMerchantProhibited(Name name, Address address)
    {
        return false;

        //VerifyMerchantIsProhibitedRequest request = new()
        //{
        //    Name = name.Value,
        //    Address = address.AsDto()
        //};

        //try
        //{
        //    HttpResponseMessage response = await _Client.PostAsJsonAsync<VerifyMerchantIsProhibitedRequest>("api/underwriting/merchant", request, _Options);

        //    VerifyResult? result = await response.Content.ReadFromJsonAsync<VerifyResult>(_Options);

        //    return result!.IsProhibited;
        //}
        //catch (Exception ex)
        //{
        //    throw new PlayInternalException(ex);
        //}
    }

    public async Task<bool> IsIndustryProhibited(MerchantCategoryCode categoryCode)
    {
        return false;

        //VerifyIndustryIsProhibitedRequest request = new() {MerchantCategoryCode = categoryCode};

        //try
        //{
        //    HttpResponseMessage response = await _Client.PostAsJsonAsync("api/underwriting/industry", request, _Options);

        //    VerifyResult? result = await response.Content.ReadFromJsonAsync<VerifyResult>(_Options);

        //    return result!.IsProhibited;
        //}
        //catch (Exception ex)
        //{
        //    throw new PlayInternalException(ex);
        //}
    }

    public async Task<bool> IsUserProhibited(PersonalDetail personalDetail, Address address, Contact contact)
    {
        return false;

        //VerifyUserIsProhibitedRequest request = new()
        //{
        //    Address = address.AsDto(),
        //    PersonalDetails = personalDetail.AsDto(),
        //    Contact = contact.AsDto()
        //};

        //try
        //{
        //    HttpResponseMessage response = await _Client.PostAsJsonAsync("api/underwriting/user", request, _Options);

        //    VerifyResult? result = await response.Content.ReadFromJsonAsync<VerifyResult>(_Options);

        //    return result!.IsProhibited;
        //}
        //catch (Exception ex)
        //{
        //    throw new PlayInternalException(ex);
        //}
    }

    #endregion
}