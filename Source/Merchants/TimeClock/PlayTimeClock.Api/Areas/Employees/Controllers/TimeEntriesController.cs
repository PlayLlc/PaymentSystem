using Microsoft.AspNetCore.Mvc;

using Play.Inventory.Api.Controllers;
using Play.Mvc.Attributes;
using Play.Mvc.Extensions;
using Play.TimeClock.Contracts.Commands;
using Play.TimeClock.Domain.Aggregates;
using Play.TimeClock.Domain.Repositories;
using Play.TimeClock.Domain.Services;

using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Attributes;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;

namespace PlayTimeClock.Api.Areas.Employees.Controllers
{
    [ApiController]
    [Area($"{nameof(Employees)}")]
    [Route("/TimeClock/[area]")]
    public class TimeEntriesController : BaseController
    {
        #region Constructor

        public TimeEntriesController(
            IRetrieveUsers userRetriever, IRetrieveMerchants merchantsRetriever, IEmployeeRepository employeeRepository,
            IEnsureEmployeeDoesNotExist uniqueEmployeeChecker) : base(userRetriever, merchantsRetriever, employeeRepository, uniqueEmployeeChecker)
        { }

        #endregion

        #region Instance Members

        [HttpPutSwagger(template: "{employeeId}/[controller]/[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTimeEntry([FromQuery] [Required] [StringLength(20)] [AlphaNumericSpecial] string employeeId, EditTimeEntry command)
        {
            this.ValidateModel();
            Employee employee = await _EmployeeRepository.GetByIdAsync(new SimpleStringId(employeeId)).ConfigureAwait(false)
                                ?? throw new NotFoundException(typeof(Employee));

            await employee.EditTimeEntry(_UserRetriever, command).ConfigureAwait(false);

            return Ok();
        }

        #endregion
    }
}