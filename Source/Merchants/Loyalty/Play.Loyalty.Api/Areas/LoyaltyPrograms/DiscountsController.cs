using Microsoft.AspNetCore.Mvc;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Loyalty.Api.Controllers;
using Play.Loyalty.Contracts.Commands;
using Play.Loyalty.Domain.Aggregates;
using Play.Mvc.Attributes;
using Play.Mvc.Extensions;

namespace Play.Loyalty.Api.Areas.LoyaltyPrograms
{
    [ApiController]
    [Area($"{nameof(LoyaltyPrograms)}")]
    [Route("/Loyalty/[area]")]
    public class DiscountsController : BaseController
    {
        #region Instance Members

        [HttpPutSwagger("{loyaltyProgramId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDiscountedItem(string loyaltyProgramId, CreateDiscountedItem command)
        {
            this.ValidateModel();
            Programs programs = await _LoyaltyProgramRepository.GetByIdAsync(new SimpleStringId(loyaltyProgramId)).ConfigureAwait(false)
                                ?? throw new NotFoundException(typeof(Programs));

            await programs.CreateDiscountedItem(_UserRetriever, command).ConfigureAwait(false);

            return Ok();
        }

        [HttpPutSwagger("{loyaltyProgramId}/[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveDiscountedItem(string loyaltyProgramId, RemoveDiscountedItem command)
        {
            this.ValidateModel();
            Programs programs = await _LoyaltyProgramRepository.GetByIdAsync(new SimpleStringId(loyaltyProgramId)).ConfigureAwait(false)
                                ?? throw new NotFoundException(typeof(Programs));

            await programs.RemoveDiscountedItem(_UserRetriever, command).ConfigureAwait(false);

            return Ok();
        }

        [HttpPutSwagger("{loyaltyProgramId}/[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateDiscountedItem(string loyaltyProgramId, UpdateDiscountedItem command)
        {
            this.ValidateModel();
            Programs programs = await _LoyaltyProgramRepository.GetByIdAsync(new SimpleStringId(loyaltyProgramId)).ConfigureAwait(false)
                                ?? throw new NotFoundException(typeof(Programs));

            await programs.UpdateDiscountedItem(_UserRetriever, command).ConfigureAwait(false);

            return Ok();
        }

        #endregion
    }
}