using Microsoft.AspNetCore.Mvc;

using Play.Identity.Domain.Serviceddds;
using Play.Loyalty.Domain.Repositories;
using Play.Loyalty.Domain.Services;

namespace Play.Loyalty.Api.Controllers
{
    [ApiController]
    [Route("/Loyalty")]
    public class BaseController : Controller
    {
        #region Instance Values

        protected readonly ILoyaltyMemberRepository _LoyaltyMemberRepository;
        protected readonly ILoyaltyProgramRepository _LoyaltyProgramRepository;
        protected readonly IEnsureUniqueRewardNumbers _UniqueRewardsNumberChecker;
        protected readonly IRetrieveUsers _UserRetriever;
        protected readonly IRetrieveMerchants _MerchantRetriever;

        #endregion

        #region Constructor

        public BaseController(
            ILoyaltyMemberRepository loyaltyMemberRepository, ILoyaltyProgramRepository loyaltyProgramRepository,
            IEnsureUniqueRewardNumbers uniqueRewardsNumberChecker, IRetrieveUsers userRetriever, IRetrieveMerchants merchantRetriever)
        {
            _LoyaltyMemberRepository = loyaltyMemberRepository;
            _LoyaltyProgramRepository = loyaltyProgramRepository;
            _UniqueRewardsNumberChecker = uniqueRewardsNumberChecker;
            _UserRetriever = userRetriever;
            _MerchantRetriever = merchantRetriever;
        }

        #endregion
    }
}