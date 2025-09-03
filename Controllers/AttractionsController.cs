using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApp.Controllers
{
    public class AttractionsController : Controller
    {
        private readonly AppDbContext _context;

        public AttractionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Attractions - доступно всем
        public IActionResult Index()
        {
            // Используем статические данные, но можно переключить на базу данных
            var staticAttractions = new List<Attraction>
            {
                new Attraction { Id = 1, Name = "Дача Башенина", Description = "Архитектурный памятник начала XX века", Address = "ул. Достоевского, 60", ImageUrl = "/images/bashinin-dacha.jpg" },
                new Attraction { Id = 2, Name = "Сарапульский краеведческий музей", Description = "Один из старейших музеев Удмуртии", Address = "ул. Первомайская, 68", ImageUrl = "/images/museum.jpg" },
                new Attraction { Id = 3, Name = "Покровская церковь", Description = "Православный храм, памятник архитектуры", Address = "ул. Труда, 6", ImageUrl = "/images/pokrovskaya-church.jpg" }
            };

            return View(staticAttractions);
            
            // Если хотите использовать базу данных, раскомментируйте:
            // return View(await _context.Attractions.ToListAsync());
        }

        // GET: Attractions/Details/5 - доступно всем
        public IActionResult Details(int id)
        {
            var staticAttractions = new List<Attraction>
            {
                new Attraction { Id = 1, Name = "Дача Башенина", Description = "Архитектурный памятник начала XX века, построенный сарапульским купцом Павлом Башениным. Представляет собой образец деревянного зодчества в стиле модерн.", Address = "ул. Достоевского, 60", ImageUrl = "/images/bashinin-dacha.jpg" },
                new Attraction { Id = 2, Name = "Сарапульский краеведческий музей", Description = "Один из старейших музеев Удмуртии, основанный в 1909 году. Содержит богатую коллекцию экспонатов по истории и природе края.", Address = "ул. Первомайская, 68", ImageUrl = "/images/museum.jpg" },
                new Attraction { Id = 3, Name = "Покровская церковь", Description = "Православный храм, построенный в 1797 году. Является памятником архитектуры федерального значения.", Address = "ул. Труда, 6", ImageUrl = "/images/pokrovskaya-church.jpg" }
            };

            var attraction = staticAttractions.FirstOrDefault(a => a.Id == id);
            if (attraction == null)
            {
                return NotFound();
            }
            return View(attraction);
            
            // Для базы данных раскомментируйте:
            /*
            var attraction = await _context.Attractions.FirstOrDefaultAsync(m => m.Id == id);
            if (attraction == null)
            {
                return NotFound();
            }
            return View(attraction);
            */
        }

        // GET: Attractions/Create - только для редакторов и администраторов
        [Authorize(Roles = "Admin,Editor")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Attractions/Create - только для редакторов и администраторов
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Create(Attraction attraction)
        {
            if (ModelState.IsValid)
            {
                // Для базы данных:
                _context.Add(attraction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                
                // Для статических данных (если хотите):
                /*
                attraction.Id = _attractions.Max(a => a.Id) + 1;
                _attractions.Add(attraction);
                return RedirectToAction(nameof(Index));
                */
            }
            return View(attraction);
        }

        // GET: Attractions/Edit/5 - только для редакторов и администраторов
        [Authorize(Roles = "Admin,Editor")]
        public IActionResult Edit(int id)
        {
            var staticAttractions = new List<Attraction>
            {
                new Attraction { Id = 1, Name = "Дача Башенина", Description = "Архитектурный памятник начала XX века", Address = "ул. Достоевского, 60", ImageUrl = "/images/bashinin-dacha.jpg" },
                new Attraction { Id = 2, Name = "Сарапульский краеведческий музей", Description = "Один из старейших музеев Удмуртии", Address = "ул. Первомайская, 68", ImageUrl = "/images/museum.jpg" },
                new Attraction { Id = 3, Name = "Покровская церковь", Description = "Православный храм, памятник архитектуры", Address = "ул. Труда, 6", ImageUrl = "/images/pokrovskaya-church.jpg" }
            };

            var attraction = staticAttractions.FirstOrDefault(a => a.Id == id);
            if (attraction == null)
            {
                return NotFound();
            }
            return View(attraction);
        }

        // POST: Attractions/Edit/5 - только для редакторов и администраторов
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Edit(int id, Attraction attraction)
        {
            if (id != attraction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Для базы данных:
                try
                {
                    _context.Update(attraction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttractionExists(attraction.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
                
                // Для статических данных:
                /*
                var existingAttraction = _attractions.FirstOrDefault(a => a.Id == id);
                if (existingAttraction != null)
                {
                    existingAttraction.Name = attraction.Name;
                    existingAttraction.Description = attraction.Description;
                    existingAttraction.Address = attraction.Address;
                    existingAttraction.ImageUrl = attraction.ImageUrl;
                }
                return RedirectToAction(nameof(Index));
                */
            }
            return View(attraction);
        }

        // GET: Attractions/Delete/5 - только для администраторов
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var staticAttractions = new List<Attraction>
            {
                new Attraction { Id = 1, Name = "Дача Башенина", Description = "Архитектурный памятник начала XX века", Address = "ул. Достоевского, 60", ImageUrl = "/images/bashinin-dacha.jpg" },
                new Attraction { Id = 2, Name = "Сарапульский краеведческий музей", Description = "Один из старейших музеев Удмуртии", Address = "ул. Первомайская, 68", ImageUrl = "/images/museum.jpg" },
                new Attraction { Id = 3, Name = "Покровская церковь", Description = "Православный храм, памятник архитектуры", Address = "ул. Труда, 6", ImageUrl = "/images/pokrovskaya-church.jpg" }
            };

            var attraction = staticAttractions.FirstOrDefault(a => a.Id == id);
            if (attraction == null)
            {
                return NotFound();
            }

            return View(attraction);
        }

        // POST: Attractions/Delete/5 - только для администраторов
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Для базы данных:
            var attraction = await _context.Attractions.FindAsync(id);
            if (attraction != null)
            {
                _context.Attractions.Remove(attraction);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
            
            // Для статических данных:
            /*
            var attraction = _attractions.FirstOrDefault(a => a.Id == id);
            if (attraction != null)
            {
                _attractions.Remove(attraction);
            }
            return RedirectToAction(nameof(Index));
            */
        }

        private bool AttractionExists(int id)
        {
            // Для базы данных:
            return _context.Attractions.Any(e => e.Id == id);
            
            // Для статических данных:
            // return _attractions.Any(e => e.Id == id);
        }

        // Статические данные для fallback (как у вас было)
        private static List<Attraction> _attractions = new()
        {
            new Attraction { Id = 1, Name = "Дача Башенина", Description = "Архитектурный памятник начала XX века", Address = "ул. Достоевского, 60", ImageUrl = "/images/bashinin-dacha.jpg" },
            new Attraction { Id = 2, Name = "Сарапульский краеведческий музей", Description = "Один из старейших музеев Удмуртии", Address = "ул. Первомайская, 68", ImageUrl = "/images/museum.jpg" },
            new Attraction { Id = 3, Name = "Покровская церковь", Description = "Православный храм, памятник архитектуры", Address = "ул. Труда, 6", ImageUrl = "/images/pokrovskaya-church.jpg" }
        };
    }
}