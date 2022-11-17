using Microsoft.AspNetCore.Mvc;

using Play.Inventory.Api.Models;

using System.Diagnostics;

namespace Play.Inventory.Api.Controllers;

[ApiController]
[Route("Inventory/[action]")]
public class HomeController : Controller
{
    #region Instance Values

    private readonly ILogger<HomeController> _logger;

    #endregion

    #region Constructor

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    #endregion

    #region Instance Members

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }

    #endregion
}