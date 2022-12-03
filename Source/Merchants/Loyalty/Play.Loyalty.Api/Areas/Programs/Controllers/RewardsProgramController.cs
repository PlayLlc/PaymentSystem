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

namespace Play.Loyalty.Api.Areas.Controllers;

[ApiController]
[Area($"{nameof(Programs)}")]
[Route("/Loyalty/[area]")]
public class RewardsProgramController : BaseController
{
    #region Constructor

    public RewardsProgramController(
        IMemberRepository memberRepository, IProgramsRepository programsRepository, IEnsureRewardsNumbersAreUnique uniqueRewardsNumberChecker,
        IRetrieveUsers userRetriever, IRetrieveMerchants merchantRetriever) : base(memberRepository, programsRepository, uniqueRewardsNumberChecker,
        userRetriever, merchantRetriever)
    { }

    #endregion

    #region Instance Members

    [HttpPutSwagger(template: "{programId}/[action]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateRewardsProgram(string programId, UpdateRewardsProgram command)
    {
        this.ValidateModel();
        Programs programs = await _ProgramsRepository.GetByIdAsync(new(programId)).ConfigureAwait(false)
                            ?? throw new NotFoundException(typeof(Programs));

        await programs.UpdateRewardsProgram(_UserRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    [HttpPutSwagger(template: "{programId}/RewardsProgram/[action]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ActivateRewardsProgram(string programId, ActivateProgram command)
    {
        this.ValidateModel();
        Programs programs = await _ProgramsRepository.GetByIdAsync(new(programId)).ConfigureAwait(false)
                            ?? throw new NotFoundException(typeof(Programs));

        await programs.ActivateRewardProgram(_UserRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    #endregion
}