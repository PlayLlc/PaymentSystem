using Microsoft.AspNetCore.Mvc;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Loyalty.Api.Controllers;
using Play.Loyalty.Contracts.Commands;
using Play.Loyalty.Domain.Aggregates;
using Play.Loyalty.Domain.Repositories;
using Play.Loyalty.Domain.Services;
using Play.Mvc.Attributes;
using Play.Mvc.Extensions;

namespace Play.Loyalty.Api.Areas.Members.Controllers;

[ApiController]
[Area($"{nameof(Members)}")]
[Route("/Loyalty/[area]/")]
public class RewardsController : BaseController
{
    #region Constructor

    public RewardsController(
        IMemberRepository memberRepository, IProgramsRepository programsRepository, IEnsureRewardsNumbersAreUnique uniqueRewardsNumberChecker,
        IRetrieveUsers userRetriever, IRetrieveMerchants merchantRetriever) : base(memberRepository, programsRepository, uniqueRewardsNumberChecker,
        userRetriever, merchantRetriever)
    { }

    #endregion

    #region Instance Members

    [HttpPutSwagger(template: "{loyaltyMemberId}/[controller]/[action]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(string loyaltyMemberId, UpdateRewardsPoints command)
    {
        this.ValidateModel();
        Member member = await _MemberRepository.GetByIdAsync(new SimpleStringId(loyaltyMemberId)).ConfigureAwait(false)
                        ?? throw new NotFoundException(typeof(Member));

        await member.AddRewardPoints(_UserRetriever, _ProgramsRepository, command).ConfigureAwait(false);

        return Ok();
    }

    [HttpPutSwagger(template: "{loyaltyMemberId}/[controller]/[action]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(string loyaltyMemberId, UpdateRewardsPoints command)
    {
        this.ValidateModel();
        Member member = await _MemberRepository.GetByIdAsync(new SimpleStringId(loyaltyMemberId)).ConfigureAwait(false)
                        ?? throw new NotFoundException(typeof(Member));

        await member.RemoveRewards(_UserRetriever, _ProgramsRepository, command).ConfigureAwait(false);

        return Ok();
    }

    [HttpPutSwagger(template: "{loyaltyMemberId}/[controller]/[action]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Claim(string loyaltyMemberId, ClaimRewards command)
    {
        this.ValidateModel();
        Member member = await _MemberRepository.GetByIdAsync(new SimpleStringId(loyaltyMemberId)).ConfigureAwait(false)
                        ?? throw new NotFoundException(typeof(Member));

        await member.ClaimRewards(_UserRetriever, _ProgramsRepository, command).ConfigureAwait(false);

        return Ok();
    }

    #endregion
}