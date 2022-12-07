using Microsoft.AspNetCore.Mvc;

using Play.Domain.Common.Dtos;
using Play.Mvc.Attributes;
using Play.Mvc.Extensions;
using Play.Underwriting.Contracts.Requests;
using Play.Underwriting.Contracts.Responses;
using Play.Underwriting.Domain.Entities;
using Play.Underwriting.Domain.Repositories;

namespace Play.Underwriting.Api.Controllers;

[SecurityHeadersAttribute]
[Route("api/[controller]")]
[ApiController]
public class UnderwritingController : ControllerBase
{
    #region Instance Values

    private readonly IUnderwritingRepository _UnderwritingRepository;

    #endregion

    #region Constructor

    public UnderwritingController(IUnderwritingRepository underwritingRepository)
    {
        _UnderwritingRepository = underwritingRepository;
    }

    #endregion

    #region Instance Members

    [HttpPost("merchant")]
    public async Task<IActionResult> VerifyMerchant([FromBody] VerifyMerchantIsProhibitedRequest request)
    {
        this.ValidateModel();

        Address merchantAddress = ToAddress(request.Address);

        bool result = await _UnderwritingRepository.IsMerchantFound(request.Name, merchantAddress);

        return Ok(new VerifyResult {IsProhibited = result});
    }

    [HttpPost("industry")]
    public async Task<IActionResult> VerifyIndustryIsProhibited([FromBody] VerifyIndustryIsProhibitedRequest request)
    {
        this.ValidateModel();

        bool result = await _UnderwritingRepository.IsIndustryFound(request.MerchantCategoryCode);

        return Ok(new VerifyResult {IsProhibited = result});
    }

    [HttpPost("user")]
    public async Task<IActionResult> VerifyUserIsProhibited([FromBody] VerifyUserIsProhibitedRequest request)
    {
        this.ValidateModel();

        Address userAddress = ToAddress(request.Address);
        string fullName = $"{request.Contact?.LastName}, {request.Contact?.FirstName}";

        bool result = await _UnderwritingRepository.IsUserFound(fullName, userAddress);

        return Ok(new VerifyResult {IsProhibited = result});
    }

    private static Address ToAddress(AddressDto? addressDto) =>
        new()
        {
            State = addressDto.State,
            City = addressDto.City,
            StreetAddress = addressDto.StreetAddress,
            ZipCode = addressDto.Zipcode
        };

    #endregion
}