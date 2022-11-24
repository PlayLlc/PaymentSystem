using Microsoft.AspNetCore.Mvc;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Identity.Domain.Serviceddds;
using Play.Loyalty.Api.Controllers;
using Play.Loyalty.Contracts.Commands;
using Play.Loyalty.Contracts.Dtos;
using Play.Loyalty.Domain.Aggregates;
using Play.Loyalty.Domain.Repositories;
using Play.Loyalty.Domain.Services;
using Play.Mvc.Attributes;
using Play.Mvc.Extensions;

namespace Play.Loyalty.Api.Areas.LoyaltyPrograms
{
    [ApiController]
    [Area($"{nameof(LoyaltyPrograms)}")]
    [Route("/Loyalty/[area]")]
    public class HomeController : BaseController
    {
        #region Constructor

        public HomeController(
            ILoyaltyMemberRepository loyaltyMemberRepository, ILoyaltyProgramRepository loyaltyProgramRepository,
            IEnsureUniqueRewardNumbers uniqueRewardsNumberChecker, IRetrieveUsers userRetriever, IRetrieveMerchants merchantRetriever) : base(
            loyaltyMemberRepository, loyaltyProgramRepository, uniqueRewardsNumberChecker, userRetriever, merchantRetriever)
        { }

        #endregion

        #region Instance Members

        [HttpPostSwagger]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateLoyaltyProgram command)
        {
            this.ValidateModel();

            Programs member = await Programs.Create(_MerchantRetriever, _UserRetriever, command).ConfigureAwait(false);

            return Created(@Url.Action("Get", "LoyaltyMembers", new {id = member.Id})!, member.AsDto());
        }

        [HttpGetSwagger("{loyaltyProgramId}/[action]")]
        [ValidateAntiForgeryToken]
        public async Task<LoyaltyProgramDto> Get(string loyaltyProgramId)
        {
            this.ValidateModel();
            Programs programs = await _LoyaltyProgramRepository.GetByIdAsync(new SimpleStringId(loyaltyProgramId)).ConfigureAwait(false)
                                ?? throw new NotFoundException(typeof(Programs));

            return programs.AsDto();
        }

        #endregion
    }
}