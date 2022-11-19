using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Play.Identity.Api.Models;

using System.Diagnostics;

using Play.Mvc.Attributes;

namespace Play.Identity.Api.Controllers;

[SwaggerIgnore]
[AllowAnonymous]
public class HomeController : Controller
{
    #region Instance Values

    private readonly IWebHostEnvironment _Environment;
    private readonly ILogger _Logger;

    #endregion

    #region Constructor

    public HomeController(IWebHostEnvironment environment, ILogger<HomeController> logger)
    {
        _Environment = environment;
        _Logger = logger;
    }

    #endregion

    #region Instance Members

    [HttpGet]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult Index()
    {
        // only show in development
        if (_Environment.IsDevelopment())
            return View();

        _Logger.LogInformation("Homepage is disabled in production. Returning 404.");

        return NotFound();
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult Privacy()
    {
        return View();
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }

    #endregion
}