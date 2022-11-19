using Microsoft.AspNetCore.Mvc;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Inventory.Contracts.Dtos;
using Play.Inventory.Domain.Repositories;
using Play.Inventory.Domain.Services;
using Play.Mvc.Extensions;

namespace Play.Inventory.Api.Controllers
{
    [ApiController]
    [Route("~/Inventory")]
    public class InventoriesController : BaseController
    {
        #region Constructor

        public InventoriesController(
            IRetrieveUsers userRetriever, IRetrieveMerchants merchantsRetriever, IItemRepository itemsRepository, ICategoryRepository categoryRepository,
            IInventoryRepository inventoryRepository) : base(userRetriever, merchantsRetriever, itemsRepository, categoryRepository, inventoryRepository)
        { }

        #endregion

        #region Instance Members

        [Route("{storeId}")]
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<InventoryDto> Index(string storeId)
        {
            this.ValidateModel();

            return (await _InventoryRepository.GetByStoreIdAsync(new SimpleStringId(storeId)).ConfigureAwait(false))?.AsDto()
                   ?? throw new NotFoundException(typeof(InventoryDto));
        }

        #endregion
    }
}