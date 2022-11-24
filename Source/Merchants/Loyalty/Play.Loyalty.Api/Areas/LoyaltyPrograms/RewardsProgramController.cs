using Microsoft.AspNetCore.Mvc;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Identity.Domain.Serviceddds;
using Play.Loyalty.Api.Controllers;
using Play.Loyalty.Contracts.Commands;
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
    public class RewardsProgramController : BaseController
    {
        #region Constructor

        public RewardsProgramController(
            ILoyaltyMemberRepository loyaltyMemberRepository, ILoyaltyProgramRepository loyaltyProgramRepository,
            IEnsureUniqueRewardNumbers uniqueRewardsNumberChecker, IRetrieveUsers userRetriever, IRetrieveMerchants merchantRetriever) : base(
            loyaltyMemberRepository, loyaltyProgramRepository, uniqueRewardsNumberChecker, userRetriever, merchantRetriever)
        { }

        #endregion

        #region Instance Members

        [HttpPutSwagger("{loyaltyProgramId}/[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRewardsProgram(string loyaltyProgramId, UpdateRewardsProgram command)
        {
            this.ValidateModel();
            Programs programs = await _LoyaltyProgramRepository.GetByIdAsync(new SimpleStringId(loyaltyProgramId)).ConfigureAwait(false)
                                ?? throw new NotFoundException(typeof(Programs));

            await programs.UpdateRewardsProgram(_UserRetriever, command).ConfigureAwait(false);

            return Ok();
        }

        [HttpPutSwagger("{loyaltyProgramId}/RewardsProgram/[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivateRewardsProgram(string loyaltyProgramId, ActivateProgram command)
        {
            this.ValidateModel();
            Programs programs = await _LoyaltyProgramRepository.GetByIdAsync(new SimpleStringId(loyaltyProgramId)).ConfigureAwait(false)
                                ?? throw new NotFoundException(typeof(Programs));

            await programs.ActivateRewardProgram(_UserRetriever, command).ConfigureAwait(false);

            return Ok();
        }

        #endregion
    }
}