using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Play.Identity.Api.Models;

using System.Diagnostics;

namespace Play.Identity.Api.Controllers;

[AllowAnonymous]
public class HomeController : Controller
{
    #region Instance Values

    private readonly IWebHostEnvironment _environment;
    private readonly ILogger _logger;

    #endregion

    #region Constructor

    public HomeController(IWebHostEnvironment environment, ILogger<HomeController> logger)
    {
        _environment = environment;
        _logger = logger;
    }

    #endregion

    #region Instance Members

    [HttpGet]
    public IActionResult Index()
    {
        // only show in development
        if (_environment.IsDevelopment())

            return View();

        _logger.LogInformation("Homepage is disabled in production. Returning 404.");

        return NotFound();
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