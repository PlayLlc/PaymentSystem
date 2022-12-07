using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Mvc.Attributes;
using Play.Mvc.Extensions;
using Play.Payroll.Api.Controllers;
using Play.Payroll.Contracts.Commands;
using Play.Payroll.Contracts.Dtos;
using Play.Payroll.Domain.Aggregates;
using Play.Payroll.Domain.Repositories;
using Play.Payroll.Domain.Services;

namespace Play.Loyalty.Api.Areas.Members.Controllers;

[Authorize]
[ApiController]
[Area($"{nameof(Members)}")]
[Route("/Payroll/[area]")]
public class HomeController : BaseController
{
    #region Instance Members

    public HomeController(IEmployerRepository employerRepository, IRetrieveUsers userRetriever, IRetrieveMerchants merchantRetriever, ISendAchTransfers achClient) : base(employerRepository, userRetriever, merchantRetriever, achClient)
    { }

    [HttpPostSwagger]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateEmployer command)
    {
        this.ValidateModel();

        var employer = await Employer.Create(_UserRetriever, _MerchantRetriever, command).ConfigureAwait(false);

        return Created(Url.Action("Get", "Home", new
        {
            area = "Employers",
            id = employer.Id
        })!, employer.AsDto());
    }

    [HttpGetSwagger(template: "{employerId}")]
    [ValidateAntiForgeryToken]
    public async Task<EmployerDto> Get(string employerId)
    {
        this.ValidateModel();
        var employer = await _EmployerRepository.GetByIdAsync(new SimpleStringId(employerId)).ConfigureAwait(false)
                       ?? throw new NotFoundException(typeof(Employer));

        return employer.AsDto();
    }

    [HttpDeleteSwagger(template: "{loyaltyMemberId}/[action]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(string loyaltyMemberId, RemoveEmployer command)
    {
        this.ValidateModel();
        Employer employer = await _EmployerRepository.GetByIdAsync(new SimpleStringId(loyaltyMemberId)).ConfigureAwait(false)
                            ?? throw new NotFoundException(typeof(Employer));

        await employer.Remove(_UserRetriever, command).ConfigureAwait(false);

        return NoContent();
    }

    #endregion
}