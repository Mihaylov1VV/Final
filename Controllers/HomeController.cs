using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyWebApp.Models;

namespace MyWebApp.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        ViewBag.WelcomeMessage = "Добро пожаловать на официальный сайт города Сарапул!";
        ViewBag.CityDescription = "Сарапул - старинный купеческий город на берегу реки Камы, с богатой историей и культурным наследием.";
        
        return View();
    }
    
    public IActionResult About()
    {
        return View();
    }
    
    public IActionResult Contact()
    {
        return View();
    }
    
    // private readonly ILogger<HomeController> _logger;
    //
    // public HomeController(ILogger<HomeController> logger)
    // {
    //     _logger = logger;
    // }
    //
    // public IActionResult Index()
    // {
    //     return View();
    // }
    //
    // public IActionResult Privacy()
    // {
    //     return View();
    // }
    //
    // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    // public IActionResult Error()
    // {
    //     return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    // }
}