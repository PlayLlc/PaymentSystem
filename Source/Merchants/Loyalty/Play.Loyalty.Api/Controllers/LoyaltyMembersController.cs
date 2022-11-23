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

[ApiController]
[Route("/Loyalty/[controller]")]
public class LoyaltyMembersController : Controller
{
    #region Instance Values

    private readonly ILoyaltyMemberRepository _LoyaltyMemberRepository;
    private readonly IEnsureUniqueRewardNumbers _UniqueRewardsNumberChecker;
    private readonly ILoyaltyProgramRepository _LoyaltyProgramRepository;
    private readonly IRetrieveUsers _UserRetriever;
    private readonly IRetrieveMerchants _MerchantRetriever;

    #endregion

    #region Constructor

    public LoyaltyMembersController(
        ILoyaltyMemberRepository loyaltyMemberRepository, ILoyaltyProgramRepository loyaltyProgramRepository,
        IEnsureUniqueRewardNumbers uniqueRewardsNumberChecker, IRetrieveUsers userRetriever, IRetrieveMerchants merchantRetriever)
    {
        _LoyaltyMemberRepository = loyaltyMemberRepository;
        _LoyaltyProgramRepository = loyaltyProgramRepository;
        _UniqueRewardsNumberChecker = uniqueRewardsNumberChecker;
        _UserRetriever = userRetriever;
        _MerchantRetriever = merchantRetriever;
    }

    #endregion

    #region Instance Members

    [HttpPostSwagger]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateLoyaltyMember command)
    {
        this.ValidateModel();

        LoyaltyMember loyaltyMember =
            await LoyaltyMember.Create(_UserRetriever, _MerchantRetriever, _UniqueRewardsNumberChecker, command).ConfigureAwait(false);

        return Created(@Url.Action("Get", "LoyaltyMembers", new {id = loyaltyMember.Id})!, loyaltyMember.AsDto());
    }

    [HttpGetSwagger("{loyaltyMemberId}")]
    [ValidateAntiForgeryToken]
    public async Task<LoyaltyMemberDto> Get(string loyaltyMemberId)
    {
        this.ValidateModel();
        var loyaltyMember = await _LoyaltyMemberRepository.GetByIdAsync(new SimpleStringId(loyaltyMemberId)).ConfigureAwait(false)
                            ?? throw new NotFoundException(typeof(LoyaltyMember));

        return loyaltyMember.AsDto();
    }

    [HttpPutSwagger("{loyaltyMemberId}/[action]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(string loyaltyMemberId, UpdateLoyaltyMember command)
    {
        this.ValidateModel();
        var loyaltyMember = await _LoyaltyMemberRepository.GetByIdAsync(new SimpleStringId(loyaltyMemberId)).ConfigureAwait(false)
                            ?? throw new NotFoundException(typeof(LoyaltyMember));
        await loyaltyMember.Update(_UserRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    [HttpDeleteSwagger("{loyaltyMemberId}/[action]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(string loyaltyMemberId, RemoveLoyaltyMember command)
    {
        this.ValidateModel();
        var loyaltyMember = await _LoyaltyMemberRepository.GetByIdAsync(new SimpleStringId(loyaltyMemberId)).ConfigureAwait(false)
                            ?? throw new NotFoundException(typeof(LoyaltyMember));
        await loyaltyMember.Remove(_UserRetriever, command).ConfigureAwait(false);

        return NoContent();
    }

    [HttpPutSwagger("{loyaltyMemberId}/[action]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddRewards(string loyaltyMemberId, UpdateRewardsPoints command)
    {
        this.ValidateModel();
        var loyaltyMember = await _LoyaltyMemberRepository.GetByIdAsync(new SimpleStringId(loyaltyMemberId)).ConfigureAwait(false)
                            ?? throw new NotFoundException(typeof(LoyaltyMember));
        await loyaltyMember.AddRewardPoints(_UserRetriever, _LoyaltyProgramRepository, command).ConfigureAwait(false);

        return Ok();
    }

    [HttpPutSwagger("{loyaltyMemberId}/[action]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveRewards(string loyaltyMemberId, UpdateRewardsPoints command)
    {
        this.ValidateModel();
        var loyaltyMember = await _LoyaltyMemberRepository.GetByIdAsync(new SimpleStringId(loyaltyMemberId)).ConfigureAwait(false)
                            ?? throw new NotFoundException(typeof(LoyaltyMember));
        await loyaltyMember.RemoveReward(_UserRetriever, _LoyaltyProgramRepository, command).ConfigureAwait(false);

        return Ok();
    }

    [HttpPutSwagger("{loyaltyMemberId}/[action]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ClaimRewards(string loyaltyMemberId, ClaimRewards command)
    {
        this.ValidateModel();
        var loyaltyMember = await _LoyaltyMemberRepository.GetByIdAsync(new SimpleStringId(loyaltyMemberId)).ConfigureAwait(false)
                            ?? throw new NotFoundException(typeof(LoyaltyMember));
        await loyaltyMember.ClaimRewards(_UserRetriever, _LoyaltyProgramRepository, command).ConfigureAwait(false);

        return Ok();
    }

    #endregion
}