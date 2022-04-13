using Microsoft.AspNetCore.Mvc;
using Notifications.UI.Models;
using Notifications.UI.RefitHttpClients;
using System.Diagnostics;

namespace Notifications.UI.Controllers;

public class HomeController : Controller
{
    private readonly IUsersApi _usersApi;
    private readonly INotificationsApi _notificationApi;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, IUsersApi usersApi, INotificationsApi notificationApi)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _usersApi = usersApi ?? throw new ArgumentNullException(nameof(usersApi));
        _notificationApi = notificationApi ?? throw new ArgumentNullException(nameof(notificationApi));
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult FindUser()
    {
        return View();
    }

    [HttpPost]
    public IActionResult FindUser(int id)
    {
        return RedirectToAction("UserDetails", new { ID = id });
    }

    public async Task<IActionResult> UserDetails(int id)
    {
        var response = await _usersApi.GetUserById(id);
        return View(response.Content);
    }

    public async Task<IActionResult> SendNotification(int id)
    {
        try
        {
            await _notificationApi.SendNotificationToUserId(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
        return RedirectToAction("UserDetails", new { ID = id });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
