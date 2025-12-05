using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AzureWebApp.Models;

namespace AzureWebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IConfiguration _configuration;

    public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public IActionResult Index()
    {
        string appName = _configuration["AppSettings:ApplicationName"];
        bool paymentEnabled = _configuration.GetValue<bool>("AppSettings:EnablePayment");
        int maxAttempts = _configuration.GetValue<int>("AppSettings:MaxLoginAttempts");
        string email = _configuration["AppSettings:SupportEmail"];

        ViewBag.AppName = appName;
        ViewBag.Payment = paymentEnabled;
        ViewBag.MaxAttempts = maxAttempts;
        ViewBag.Email = email;
        
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
