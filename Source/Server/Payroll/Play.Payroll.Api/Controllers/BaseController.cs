using Microsoft.AspNetCore.Mvc;

using Play.Payroll.Domain.Repositories;
using Play.Payroll.Domain.Services;

namespace Play.Payroll.Api.Controllers;

[ApiController]
[Route("/Payroll")]
public class BaseController : Controller
{
    #region Instance Values

    protected readonly IEmployerRepository _EmployerRepository;
    protected readonly IRetrieveUsers _UserRetriever;
    protected readonly IRetrieveMerchants _MerchantRetriever;
    protected readonly ISendAchTransfers _AchClient;

    #endregion

    #region Constructor

    public BaseController(
        IEmployerRepository employerRepository, IRetrieveUsers userRetriever, IRetrieveMerchants merchantRetriever, ISendAchTransfers achClient)
    {
        _EmployerRepository = employerRepository;
        _UserRetriever = userRetriever;
        _MerchantRetriever = merchantRetriever;
        _AchClient = achClient;
    }

    #endregion
}