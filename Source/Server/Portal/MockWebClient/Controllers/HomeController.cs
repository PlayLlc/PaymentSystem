using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;

using MockWebClient.Models;

namespace MockWebClient.Controllers;

public class HomeController : Controller
{
    #region Instance Values

    private readonly ILogger<HomeController> _Logger;

    #endregion

    #region Constructor

    public HomeController(ILogger<HomeController> logger)
    {
        _Logger = logger;
    }

    #endregion

    #region Instance Members

    public IActionResult Index() => View();

    public IActionResult Logout() => SignOut("Cookies", "oidc");

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});

    #endregion
}