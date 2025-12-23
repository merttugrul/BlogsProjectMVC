using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using APP.Services; 

namespace MVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRecentBlogsService _recentBlogsService;

    public HomeController(ILogger<HomeController> logger, IRecentBlogsService recentBlogsService)
    {
        _logger = logger;
        _recentBlogsService = recentBlogsService;
    }

    public IActionResult Index()
    {
        // Get recently viewed blogs from session
        var recentBlogs = _recentBlogsService.GetRecentBlogs();
        ViewBag.RecentBlogs = recentBlogs;
        
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