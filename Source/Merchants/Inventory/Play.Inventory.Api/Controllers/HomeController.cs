using Microsoft.AspNetCore.Mvc;

using Play.Inventory.Api.Models;

using System.Diagnostics;

using Play.Inventory.Domain.Repositories;
using Play.Inventory.Domain.Services;

namespace Play.Inventory.Api.Controllers;

[ApiController]
[Route("[action]")]
public class HomeController : InventoryController
{
    #region Constructor

    public HomeController(
        IRetrieveUsers userRetriever, IRetrieveMerchants merchantsRetriever, IItemRepository itemsRepository, ICategoryRepository categoryRepository) : base(
        userRetriever, merchantsRetriever, itemsRepository, categoryRepository)
    { }

    #endregion

    #region Instance Members

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Privacy()
    {
        return View();
    }

    [HttpGet]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }

    #endregion
}