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

namespace Play.Loyalty.Api.Areas.Members;

[ApiController]
[Area($"{nameof(Members)}")]
[Route("/Loyalty/[area]/")]
public class RewardsController : BaseController
{
    #region Constructor

    public RewardsController(
        ILoyaltyMemberRepository loyaltyMemberRepository, ILoyaltyProgramRepository loyaltyProgramRepository,
        IEnsureUniqueRewardNumbers uniqueRewardsNumberChecker, IRetrieveUsers userRetriever, IRetrieveMerchants merchantRetriever) : base(
        loyaltyMemberRepository, loyaltyProgramRepository, uniqueRewardsNumberChecker, userRetriever, merchantRetriever)
    { }

    #endregion

    #region Instance Members

    [HttpPutSwagger(template: "{loyaltyMemberId}/[controller]/[action]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddRewards(string loyaltyMemberId, UpdateRewardsPoints command)
    {
        this.ValidateModel();
        Member member = await _LoyaltyMemberRepository.GetByIdAsync(new SimpleStringId(loyaltyMemberId)).ConfigureAwait(false)
                        ?? throw new NotFoundException(typeof(Member));

        await member.AddRewardPoints(_UserRetriever, _LoyaltyProgramRepository, command).ConfigureAwait(false);

        return Ok();
    }

    [HttpPutSwagger(template: "{loyaltyMemberId}/[controller]/[action]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveRewards(string loyaltyMemberId, UpdateRewardsPoints command)
    {
        this.ValidateModel();
        Member member = await _LoyaltyMemberRepository.GetByIdAsync(new SimpleStringId(loyaltyMemberId)).ConfigureAwait(false)
                        ?? throw new NotFoundException(typeof(Member));

        await member.RemoveRewards(_UserRetriever, _LoyaltyProgramRepository, command).ConfigureAwait(false);

        return Ok();
    }

    [HttpPutSwagger(template: "{loyaltyMemberId}/[controller]/[action]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ClaimRewards(string loyaltyMemberId, ClaimRewards command)
    {
        this.ValidateModel();
        Member member = await _LoyaltyMemberRepository.GetByIdAsync(new SimpleStringId(loyaltyMemberId)).ConfigureAwait(false)
                        ?? throw new NotFoundException(typeof(Member));

        await member.ClaimRewards(_UserRetriever, _LoyaltyProgramRepository, command).ConfigureAwait(false);

        return Ok();
    }

    #endregion
}