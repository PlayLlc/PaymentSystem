using Microsoft.AspNetCore.Mvc;

using Play.Inventory.Domain.Repositories;
using Play.Inventory.Domain.Services;

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

        #endregion

        #region Constructor

        public InventoryController(
            IRetrieveUsers userRetriever, IRetrieveMerchants merchantsRetriever, IItemRepository itemsRepository, ICategoryRepository categoryRepository)
        {
            _UserRetriever = userRetriever;
            _MerchantsRetriever = merchantsRetriever;
            _ItemsRepository = itemsRepository;
            _CategoryRepository = categoryRepository;
        }

        #endregion
    }
}