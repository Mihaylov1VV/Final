using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyWebApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApp.Controllers;

public class AttractionsController : Controller
{
    private static List<Attraction> _attractions = new()
    {
        new Attraction { Id = 1, Name = "Дача Башенина", Description = "Архитектурный памятник начала XX века", Address = "ул. Достоевского, 60" },
        new Attraction { Id = 2, Name = "Сарапульский краеведческий музей", Description = "Один из старейших музеев Удмуртии", Address = "ул. Первомайская, 68" }
    };
    
    public IActionResult Index()
    {
        return View(_attractions);
    }
    
    public IActionResult Details(int id)
    {
        var attraction = _attractions.FirstOrDefault(a => a.Id == id);
        if (attraction == null)
        {
            return NotFound();
        }
        return View(attraction);
    }
}