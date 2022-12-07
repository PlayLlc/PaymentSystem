using Microsoft.AspNetCore.Mvc;

using Play.Loyalty.Domain.Repositories;
using Play.Loyalty.Domain.Services;

namespace Play.Loyalty.Api.Controllers;

[ApiController]
[Route("/Loyalty")]
public class BaseController : Controller
{
    #region Instance Values

    protected readonly IMemberRepository _MemberRepository;
    protected readonly IProgramsRepository _ProgramsRepository;
    protected readonly IEnsureRewardsNumbersAreUnique _UniqueRewardsNumberChecker;
    protected readonly IRetrieveUsers _UserRetriever;
    protected readonly IRetrieveMerchants _MerchantRetriever;

    #endregion

    #region Constructor

    public BaseController(
        IMemberRepository memberRepository, IProgramsRepository programsRepository, IEnsureRewardsNumbersAreUnique uniqueRewardsNumberChecker,
        IRetrieveUsers userRetriever, IRetrieveMerchants merchantRetriever)
    {
        _MemberRepository = memberRepository;
        _ProgramsRepository = programsRepository;
        _UniqueRewardsNumberChecker = uniqueRewardsNumberChecker;
        _UserRetriever = userRetriever;
        _MerchantRetriever = merchantRetriever;
    }

    #endregion
}