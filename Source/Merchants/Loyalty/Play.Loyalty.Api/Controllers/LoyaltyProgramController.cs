using Microsoft.AspNetCore.Mvc;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Identity.Domain.Serviceddds;
using Play.Loyalty.Contracts.Commands;
using Play.Loyalty.Contracts.Dtos;
using Play.Loyalty.Domain.Aggregates;
using Play.Loyalty.Domain.Repositories;
using Play.Loyalty.Domain.Services;
using Play.Mvc.Attributes;
using Play.Mvc.Extensions;

namespace Play.Loyalty.Api.Controllers;

public class LoyaltyProgramController : Controller
{
    #region Instance Values

    private readonly ILoyaltyProgramRepository _LoyaltyProgramRepository;
    private readonly IRetrieveUsers _UserRetriever;
    private readonly IRetrieveMerchants _MerchantRetriever;

    #endregion

    #region Constructor

    public LoyaltyProgramController(ILoyaltyProgramRepository loyaltyProgramRepository, IRetrieveUsers userRetriever, IRetrieveMerchants merchantRetriever)
    {
        _LoyaltyProgramRepository = loyaltyProgramRepository;
        _UserRetriever = userRetriever;
        _MerchantRetriever = merchantRetriever;
    }

    #endregion

    #region Instance Members

    [HttpPostSwagger]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateLoyaltyProgram command)
    {
        this.ValidateModel();

        LoyaltyProgram loyaltyMember = await LoyaltyProgram.Create(_MerchantRetriever, _UserRetriever, command).ConfigureAwait(false);

        return Created(@Url.Action("Get", "LoyaltyMembers", new {id = loyaltyMember.Id})!, loyaltyMember.AsDto());
    }

    [HttpGetSwagger("{loyaltyProgramId}/[action]")]
    [ValidateAntiForgeryToken]
    public async Task<LoyaltyProgramDto> Get(string loyaltyProgramId)
    {
        this.ValidateModel();
        LoyaltyProgram loyaltyProgram = await _LoyaltyProgramRepository.GetByIdAsync(new SimpleStringId(loyaltyProgramId)).ConfigureAwait(false)
                                        ?? throw new NotFoundException(typeof(LoyaltyProgram));

        return loyaltyProgram.AsDto();
    }

    [HttpPutSwagger("{loyaltyProgramId}/[action]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateRewardsProgram(string loyaltyProgramId, UpdateRewardsProgram command)
    {
        this.ValidateModel();
        LoyaltyProgram loyaltyProgram = await _LoyaltyProgramRepository.GetByIdAsync(new SimpleStringId(loyaltyProgramId)).ConfigureAwait(false)
                                        ?? throw new NotFoundException(typeof(LoyaltyProgram));

        await loyaltyProgram.UpdateRewardsProgram(_UserRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    [HttpPutSwagger("{loyaltyProgramId}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateDiscountedItem(string loyaltyProgramId, CreateDiscountedItem command)
    {
        this.ValidateModel();
        LoyaltyProgram loyaltyProgram = await _LoyaltyProgramRepository.GetByIdAsync(new SimpleStringId(loyaltyProgramId)).ConfigureAwait(false)
                                        ?? throw new NotFoundException(typeof(LoyaltyProgram));

        await loyaltyProgram.CreateDiscountedItem(_UserRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    [HttpPutSwagger("{loyaltyProgramId}/[action]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveDiscountedItem(string loyaltyProgramId, RemoveDiscountedItem command)
    {
        this.ValidateModel();
        LoyaltyProgram loyaltyProgram = await _LoyaltyProgramRepository.GetByIdAsync(new SimpleStringId(loyaltyProgramId)).ConfigureAwait(false)
                                        ?? throw new NotFoundException(typeof(LoyaltyProgram));

        await loyaltyProgram.RemoveDiscountedItem(_UserRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    [HttpPutSwagger("{loyaltyProgramId}/[action]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateDiscountedItem(string loyaltyProgramId, UpdateDiscountedItem command)
    {
        this.ValidateModel();
        LoyaltyProgram loyaltyProgram = await _LoyaltyProgramRepository.GetByIdAsync(new SimpleStringId(loyaltyProgramId)).ConfigureAwait(false)
                                        ?? throw new NotFoundException(typeof(LoyaltyProgram));

        await loyaltyProgram.UpdateDiscountedItem(_UserRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    [HttpPutSwagger("{loyaltyProgramId}/[action]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ActivateRewardsProgram(string loyaltyProgramId, ActivateRewardsProgram command)
    {
        this.ValidateModel();
        LoyaltyProgram loyaltyProgram = await _LoyaltyProgramRepository.GetByIdAsync(new SimpleStringId(loyaltyProgramId)).ConfigureAwait(false)
                                        ?? throw new NotFoundException(typeof(LoyaltyProgram));

        await loyaltyProgram.ActivateRewardsProgram(_UserRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    #endregion
}