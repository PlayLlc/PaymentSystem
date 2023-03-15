using Microsoft.AspNetCore.Mvc;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Mvc.Attributes;
using Play.Mvc.Extensions;
using Play.Payroll.Api.Controllers;
using Play.Payroll.Contracts.Commands;
using Play.Payroll.Domain.Aggregates;
using Play.Payroll.Domain.Repositories;
using Play.Payroll.Domain.Services;

namespace Play.Payroll.Api.Areas.Employers.Controllers;

[ApiController]
[Area($"{nameof(Employers)}")]
[Route("/Payroll/[area]/")]
public class EmployeesController : BaseController
{
    #region Constructor

    public EmployeesController(
        IEmployerRepository employerRepository, IRetrieveUsers userRetriever, IRetrieveMerchants merchantRetriever, ISendAchTransfers achClient) : base(
        employerRepository, userRetriever, merchantRetriever, achClient)
    { }

    #endregion

    #region Instance Members

    [HttpPutSwagger(template: "{employerId}/[controller]/[action]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(string employerId, CreateEmployee command)
    {
        this.ValidateModel();
        Employer? employer = await _EmployerRepository.GetByIdAsync(new SimpleStringId(employerId)).ConfigureAwait(false)
                             ?? throw new NotFoundException(typeof(Employer));
        await employer.CreateEmployee(_UserRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    [HttpPutSwagger(template: "{employerId}/[controller]/[action]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(string employerId, RemoveEmployee command)
    {
        this.ValidateModel();
        Employer? employer = await _EmployerRepository.GetByIdAsync(new SimpleStringId(employerId)).ConfigureAwait(false)
                             ?? throw new NotFoundException(typeof(Employer));

        await employer.RemoveEmployee(_UserRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    [HttpPutSwagger(template: "{employerId}/[controller]/[action]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Compensation(string employerId, UpdateEmployeeCompensation command)
    {
        this.ValidateModel();
        Employer? employer = await _EmployerRepository.GetByIdAsync(new SimpleStringId(employerId)).ConfigureAwait(false)
                             ?? throw new NotFoundException(typeof(Employer));

        await employer.UpdateCompensation(_UserRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    [HttpPutSwagger(template: "{employerId}/[controller]/[action]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TimeEntry(string employerId, UpdateTimeEntry command)
    {
        this.ValidateModel();
        Employer? employer = await _EmployerRepository.GetByIdAsync(new SimpleStringId(employerId)).ConfigureAwait(false)
                             ?? throw new NotFoundException(typeof(Employer));
        await employer.UpdateTimeEntry(_UserRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    #endregion
}