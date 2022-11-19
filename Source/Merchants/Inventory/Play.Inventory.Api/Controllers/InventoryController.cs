using Microsoft.AspNetCore.Mvc;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Inventory.Contracts.Dtos;
using Play.Inventory.Domain.Aggregates;
using Play.Inventory.Domain.Repositories;
using Play.Inventory.Domain.Services;
using Play.Mvc.Extensions;

namespace Play.Inventory.Api.Controllers
{
    [ApiController]
    [Route("/Inventory")]
    public class InventoryController : Controller
    {
        #region Instance Values

        protected readonly IRetrieveUsers _UserRetriever;
        protected readonly IRetrieveMerchants _MerchantsRetriever;
        protected readonly IItemRepository _ItemsRepository;
        protected readonly ICategoryRepository _CategoryRepository;
        protected readonly IInventoryRepository _InventoryRepository;

        #endregion

        #region Constructor

        public InventoryController(
            IRetrieveUsers userRetriever, IRetrieveMerchants merchantsRetriever, IItemRepository itemsRepository, ICategoryRepository categoryRepository,
            IInventoryRepository inventoryRepository)
        {
            _UserRetriever = userRetriever;
            _MerchantsRetriever = merchantsRetriever;
            _ItemsRepository = itemsRepository;
            _CategoryRepository = categoryRepository;
            _InventoryRepository = inventoryRepository;
        }

        #endregion

        #region Instance Members

        [Route("{storeId=storeId:string")]
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