using Microsoft.AspNetCore.Authorization;
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

[Authorize]
[ApiController]
[Area($"{nameof(Programs)}")]
[Route("/Loyalty/[area]")]
public class DiscountsController : BaseController
{
    #region Instance Values

    private readonly IRetrieveInventoryItems _InventoryItemRetriever;

    #endregion

    #region Constructor

    public DiscountsController(
        IMemberRepository memberRepository, IProgramsRepository programsRepository, IEnsureRewardsNumbersAreUnique uniqueRewardsNumberChecker,
        IRetrieveUsers userRetriever, IRetrieveMerchants merchantRetriever, IRetrieveInventoryItems inventoryItemRetriever) : base(memberRepository,
        programsRepository, uniqueRewardsNumberChecker, userRetriever, merchantRetriever)
    {
        _InventoryItemRetriever = inventoryItemRetriever;
    }

    #endregion

    #region Instance Members

    [HttpPutSwagger(template: "{programsId}/[controller]/[action]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(string programsId, CreateDiscountedItem command)
    {
        this.ValidateModel();
        Programs programs = await _ProgramsRepository.GetByIdAsync(new SimpleStringId(programsId)).ConfigureAwait(false)
                            ?? throw new NotFoundException(typeof(Programs));

        await programs.CreateDiscountedItem(_UserRetriever, _InventoryItemRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    [HttpPutSwagger(template: "{programsId}/[controller]/[action]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(string programsId, RemoveDiscountedItem command)
    {
        this.ValidateModel();
        Programs programs = await _ProgramsRepository.GetByIdAsync(new SimpleStringId(programsId)).ConfigureAwait(false)
                            ?? throw new NotFoundException(typeof(Programs));

        await programs.RemoveDiscountedItem(_UserRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    [HttpPutSwagger(template: "{programsId}/[controller]/[action]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(string programsId, UpdateDiscountedItem command)
    {
        this.ValidateModel();
        Programs programs = await _ProgramsRepository.GetByIdAsync(new SimpleStringId(programsId)).ConfigureAwait(false)
                            ?? throw new NotFoundException(typeof(Programs));

        await programs.UpdateDiscountedItem(_UserRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    #endregion
}