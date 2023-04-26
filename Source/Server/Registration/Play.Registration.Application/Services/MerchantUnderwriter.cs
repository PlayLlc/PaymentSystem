using System.Text.Json;

using Microsoft.Extensions.Options;

using Play.Domain.Common.Entities;
using Play.Domain.Common.ValueObjects;
using Play.Registration.Domain.Entities;
using Play.Registration.Domain.Services;
using Play.Registration.Domain.ValueObjects;

namespace Play.Registration.Application.Services;

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

    public async Task<bool> IsMerchantProhibited(Name name, Address address) => false;

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
    public async Task<bool> IsIndustryProhibited(MerchantCategoryCode categoryCode) => false;

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
    public async Task<bool> IsUserProhibited(PersonalDetail personalDetail, Address address, Contact contact) => false;

    #endregion

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