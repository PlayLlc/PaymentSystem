using Microsoft.AspNetCore.Mvc;

using Play.Payroll.Api.Controllers;
using Play.Payroll.Domain.Repositories;
using Play.Payroll.Domain.Services;

namespace Play.Payroll.Api.Areas.Employers.Controllers;

public class PaychecksController : BaseController
{
    #region Constructor

    public PaychecksController(
        IEmployerRepository employerRepository, IRetrieveUsers userRetriever, IRetrieveMerchants merchantRetriever, ISendAchTransfers achClient) : base(
        employerRepository, userRetriever, merchantRetriever, achClient)
    { }

    #endregion
}