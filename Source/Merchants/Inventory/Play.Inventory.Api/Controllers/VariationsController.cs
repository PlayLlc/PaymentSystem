using Microsoft.AspNetCore.Mvc;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Repositories;
using Play.Inventory.Domain;

namespace Play.Inventory.Api.Controllers;

[ApiController]
[Route("Inventory/[controller]/[action]")]
public class VariationsController : Controller
{
    #region Instance Values

    private readonly IRepository<Item, SimpleStringId> _ItemsRepository;

    private readonly ILogger<CategoriesController> _Logger;

    #endregion

    #region Constructor

    public VariationsController(IRepository<Item, SimpleStringId> itemsRepository, ILogger<CategoriesController> logger)
    {
        _ItemsRepository = itemsRepository;
        _Logger = logger;
    }

    #endregion

    //public IActionResult Index()
    //{
    //    return View();
    //}
}