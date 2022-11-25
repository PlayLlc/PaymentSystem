using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Identity.Domain.Serviceddds;
using Play.Loyalty.Api.Controllers;
using Play.Loyalty.Contracts.Commands;
using Play.Loyalty.Domain.Aggregates;
using Play.Loyalty.Domain.Repositories;
using Play.Loyalty.Domain.Services;
using Play.Mvc.Attributes;
using Play.Mvc.Extensions;

namespace Play.Loyalty.Api.Areas.LoyaltyPrograms;

[Authorize]
[ApiController]
[Area($"{nameof(Programs)}")]
[Route("/Loyalty/[area]")]
public class DiscountsController : BaseController
{
    #region Constructor

    public DiscountsController(
        ILoyaltyMemberRepository loyaltyMemberRepository, ILoyaltyProgramRepository loyaltyProgramRepository,
        IEnsureUniqueRewardNumbers uniqueRewardsNumberChecker, IRetrieveUsers userRetriever, IRetrieveMerchants merchantRetriever) : base(
        loyaltyMemberRepository, loyaltyProgramRepository, uniqueRewardsNumberChecker, userRetriever, merchantRetriever)
    { }

    #endregion

    #region Instance Members

    [HttpPutSwagger(template: "{programsId}/[controller]/[action]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(string programsId, CreateDiscountedItem command)
    {
        this.ValidateModel();
        Programs programs = await _LoyaltyProgramRepository.GetByIdAsync(new SimpleStringId(programsId)).ConfigureAwait(false)
                            ?? throw new NotFoundException(typeof(Programs));

        await programs.CreateDiscountedItem(_UserRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    [HttpPutSwagger(template: "{programsId}/[controller]/[action]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(string programsId, RemoveDiscountedItem command)
    {
        this.ValidateModel();
        Programs programs = await _LoyaltyProgramRepository.GetByIdAsync(new SimpleStringId(programsId)).ConfigureAwait(false)
                            ?? throw new NotFoundException(typeof(Programs));

        await programs.RemoveDiscountedItem(_UserRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    [HttpPutSwagger(template: "{programsId}/[controller]/[action]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(string programsId, UpdateDiscountedItem command)
    {
        this.ValidateModel();
        Programs programs = await _LoyaltyProgramRepository.GetByIdAsync(new SimpleStringId(programsId)).ConfigureAwait(false)
                            ?? throw new NotFoundException(typeof(Programs));

        await programs.UpdateDiscountedItem(_UserRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    #endregion
}