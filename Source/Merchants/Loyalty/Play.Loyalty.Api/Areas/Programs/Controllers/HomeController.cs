using Microsoft.AspNetCore.Mvc;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Loyalty.Api.Controllers;
using Play.Loyalty.Contracts.Commands;
using Play.Loyalty.Contracts.Dtos;
using Play.Loyalty.Domain.Aggregates;
using Play.Loyalty.Domain.Repositories;
using Play.Loyalty.Domain.Services;
using Play.Mvc.Attributes;
using Play.Mvc.Extensions;

namespace Play.Loyalty.Api.Areas.Controllers;

[ApiController]
[Area($"{nameof(Programs)}")]
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
    public async Task<IActionResult> Create(CreateLoyaltyProgram command)
    {
        this.ValidateModel();

        Programs programs = await Programs.Create(_MerchantRetriever, _UserRetriever, command).ConfigureAwait(false);

        return Created(Url.Action("Get", "Home", new
        {
            area = nameof(Programs),
            id = programs.Id
        })!, programs.AsDto());
    }

    [HttpGetSwagger(template: "{programId}")]
    [ValidateAntiForgeryToken]
    public async Task<LoyaltyProgramDto> Get(string programsId)
    {
        this.ValidateModel();
        Programs programs = await _ProgramsRepository.GetByIdAsync(new SimpleStringId(programsId)).ConfigureAwait(false)
                            ?? throw new NotFoundException(typeof(Programs));

        return programs.AsDto();
    }

    #endregion
}