﻿using Microsoft.AspNetCore.Mvc;
using Play.Accounts.Contracts.Dtos;
using Play.Mvc.Attributes;
using Play.Mvc.Extensions;

using Play.Underwriting.Contracts.Requests;
using Play.Underwriting.Domain.Entities;
using Play.Underwriting.Domain.Repositories;

namespace Play.Underwriting.Api.Controllers;

[SecurityHeadersAttribute]
[Route("api/[controller]")]
[ApiController]
public class UnderwritingController : ControllerBase
{
    private readonly IUnderwritingRepository _UnderwritingRepository;

    public UnderwritingController(IUnderwritingRepository underwritingRepository)
    {
        _UnderwritingRepository = underwritingRepository;
    }

    [HttpPost]
    public async Task<IActionResult> VerifyMerchant([FromBody] VerifyMerchantIsProhibitedRequest request)
    {
        this.ValidateModel();

        Address merchantAddress = ToAddress(request.Address);

        var result = await _UnderwritingRepository.IsMerchantFound(request.Name, merchantAddress);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> VerifyIndustryIsProhibited([FromBody] VerifyIndustryIsProhibitedRequest request)
    {
        this.ValidateModel();

        var result = await _UnderwritingRepository.IsIndustryFound(request.MerchantCategoryCode);

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> VerifyUserIsProhibited([FromBody] VerifyUserIsProhibitedRequest request)
    {
        this.ValidateModel();

        Address userAddress = ToAddress(request.Address);
        string fullName = $"{request.Contact?.LastName}, {request.Contact?.FirstName}";

        var result = await _UnderwritingRepository.IsUserFound(fullName, userAddress);

        return Ok(result);
    }

    private static Address ToAddress(AddressDto? addressDto)
    {
        return new Address
        {
            State = addressDto.State,
            City = addressDto.City,
            StreetAddress = addressDto.StreetAddress,
            ZipCode = addressDto.Zipcode
        };
    }
}
