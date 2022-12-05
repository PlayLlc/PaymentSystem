using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Play.Domain.Exceptions;
using Play.Mvc.Attributes;
using Play.Mvc.Extensions;
using Play.Payroll.Api.Controllers;
using Play.Payroll.Domain.Services;

namespace Play.Loyalty.Api.Areas.Members.Controllers;

[Authorize]
[ApiController]
[Area($"{nameof(Members)}")]
[Route("/Loyalty/[area]")]
public class HomeController : BaseController
{
    #region Constructor

    public HomeController(
        IMemberRepository memberRepository, IProgramsRepository programsRepository, IEnsureRewardsNumbersAreUnique uniqueRewardsNumberChecker,
        IRetrieveUsers userRetriever, IRetrieveMerchants merchantRetriever) : base(memberRepository, programsRepository, uniqueRewardsNumberChecker,
        userRetriever, merchantRetriever)
    { }

    #endregion

    #region Instance Members

    [HttpPostSwagger]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateLoyaltyMember command)
    {
        this.ValidateModel();

        Member member = await Member.Create(_UserRetriever, _MerchantRetriever, _UniqueRewardsNumberChecker, command).ConfigureAwait(false);

        return Created(Url.Action("Get", "Home", new
        {
            area = nameof(Members),
            id = member.Id
        })!, member.AsDto());
    }

    [HttpGetSwagger(template: "{loyaltyMemberId}")]
    [ValidateAntiForgeryToken]
    public async Task<LoyaltyMemberDto> Get(string loyaltyMemberId)
    {
        this.ValidateModel();
        Member member = await _MemberRepository.GetByIdAsync(new(loyaltyMemberId)).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Member));

        return member.AsDto();
    }

    [HttpPutSwagger(template: "{loyaltyMemberId}/[action]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(string loyaltyMemberId, UpdateLoyaltyMember command)
    {
        this.ValidateModel();
        Member member = await _MemberRepository.GetByIdAsync(new(loyaltyMemberId)).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Member));

        await member.Update(_UserRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    [HttpDeleteSwagger(template: "{loyaltyMemberId}/[action]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(string loyaltyMemberId, RemoveLoyaltyMember command)
    {
        this.ValidateModel();
        Member member = await _MemberRepository.GetByIdAsync(new(loyaltyMemberId)).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Member));

        await member.Remove(_UserRetriever, command).ConfigureAwait(false);

        return NoContent();
    }

    #endregion
}