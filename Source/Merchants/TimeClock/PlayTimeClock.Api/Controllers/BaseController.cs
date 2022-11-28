using Microsoft.AspNetCore.Mvc;

using Play.TimeClock.Domain.Repositories;
using Play.TimeClock.Domain.Services;

namespace PlayTimeClock.Api.Controllers;

[ApiController]
[Route("/TimeClock")]
public class BaseController : Controller
{
    #region Instance Values

    protected readonly IRetrieveUsers _UserRetriever;
    protected readonly IRetrieveMerchants _MerchantsRetriever;
    protected readonly IEmployeeRepository _EmployeeRepository;
    protected readonly IEnsureEmployeeDoesNotExist _UniqueEmployeeChecker;

    #endregion

    #region Constructor

    public BaseController(
        IRetrieveUsers userRetriever, IRetrieveMerchants merchantsRetriever, IEmployeeRepository employeeRepository,
        IEnsureEmployeeDoesNotExist uniqueEmployeeChecker)
    {
        _UserRetriever = userRetriever;
        _MerchantsRetriever = merchantsRetriever;
        _EmployeeRepository = employeeRepository;
        _UniqueEmployeeChecker = uniqueEmployeeChecker;
    }

    #endregion
}