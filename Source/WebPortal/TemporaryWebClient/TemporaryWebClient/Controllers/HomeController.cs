using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

using TemporaryWebClient.Models;

namespace TemporaryWebClient.Controllers;

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
        var contactUsModel = new ContactUsModel();

        return View(contactUsModel);
    }

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});

    #endregion
}