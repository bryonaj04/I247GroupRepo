using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using I247GroupTest.Models;
using I247GroupTest.Services;
using I247GroupTest.Interfaces;

namespace I247GroupTest.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRandomUserApiService _randomUserApiService;


    public HomeController(ILogger<HomeController> logger, IRandomUserApiService randomUserApiService)
    {
        _logger = logger;
        _randomUserApiService = randomUserApiService;
    }

    public async Task <IActionResult> Index()
    {
        var response = await _randomUserApiService.GetRandomUserDataFromApi();
        return View(response);
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

