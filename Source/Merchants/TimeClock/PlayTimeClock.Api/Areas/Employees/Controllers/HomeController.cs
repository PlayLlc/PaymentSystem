using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

using Play.Domain.Common.Attributes;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Mvc.Attributes;
using Play.Mvc.Extensions;
using Play.TimeClock.Contracts.Commands;
using Play.TimeClock.Contracts.Dtos;
using Play.TimeClock.Domain.Aggregates;
using Play.TimeClock.Domain.Repositories;
using Play.TimeClock.Domain.Services;

using PlayTimeClock.Api.Controllers;

namespace PlayTimeClock.Api.Areas.Employees.Controllers
{
    public class HomeController : BaseController
    {
        #region Constructor

        public HomeController(
            IRetrieveUsers userRetriever, IRetrieveMerchants merchantsRetriever, IEmployeeRepository employeeRepository,
            IEnsureEmployeeDoesNotExist uniqueEmployeeChecker) : base(userRetriever, merchantsRetriever, employeeRepository, uniqueEmployeeChecker)
        { }

        #endregion

        #region Instance Members

        [HttpPostSwagger]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEmployee command)
        {
            this.ValidateModel();

            Employee employee = await Employee.Create(_UserRetriever, _MerchantsRetriever, _UniqueEmployeeChecker, command).ConfigureAwait(false);

            return Created(@Url.Action("Get", "Home", new {id = employee.Id})!, employee.AsDto());
        }

        [HttpGetSwagger(template: "{employeeId}")]
        [ValidateAntiForgeryToken]
        public async Task<EmployeeDto> Get([Required] [StringLength(20)] [AlphaNumericSpecial] string employeeId)
        {
            Employee employee = await _EmployeeRepository.GetByIdAsync(new SimpleStringId(employeeId)).ConfigureAwait(false)
                                ?? throw new NotFoundException(typeof(Employee));

            return employee.AsDto();
        }

        [HttpDeleteSwagger(template: "{employeeId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove([Required] [StringLength(20)] [AlphaNumericSpecial] string employeeId, RemoveEmployee command)
        {
            this.ValidateModel();
            Employee employee = await _EmployeeRepository.GetByIdAsync(new SimpleStringId(employeeId)).ConfigureAwait(false)
                                ?? throw new NotFoundException(typeof(Employee));

            await employee.Remove(_UserRetriever, command).ConfigureAwait(false);

            return NoContent();
        }

        [HttpPutSwagger(template: "{employeeId}/[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClockIn([Required] [StringLength(20)] [AlphaNumericSpecial] string employeeId, UpdateTimeClock command)
        {
            this.ValidateModel();
            Employee employee = await _EmployeeRepository.GetByIdAsync(new SimpleStringId(employeeId)).ConfigureAwait(false)
                                ?? throw new NotFoundException(typeof(Employee));

            await employee.ClockIn(_UserRetriever, command).ConfigureAwait(false);

            return Ok();
        }

        [HttpPutSwagger(template: "{employeeId}/[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClockOut([Required] [StringLength(20)] [AlphaNumericSpecial] string employeeId, UpdateTimeClock command)
        {
            this.ValidateModel();
            Employee employee = await _EmployeeRepository.GetByIdAsync(new SimpleStringId(employeeId)).ConfigureAwait(false)
                                ?? throw new NotFoundException(typeof(Employee));

            await employee.ClockOut(_UserRetriever, command).ConfigureAwait(false);

            return Ok();
        }

        #endregion
    }
}