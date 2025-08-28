// namespace MyWebApp.Controllers;
//
// public class NewsController
// {
//     private static List<NewsItem> _news = new()
//     {
//         new NewsItem { Id = 1, Title = "Фестиваль 'Сарапул - город на Каме'", Content = "В выходные состоится ежегодный фестиваль...", PublishDate = DateTime.Now.AddDays(-2) },
//         new NewsItem { Id = 2, Title = "Открытие нового парка", Content = "На следующей неделе планируется открытие...", PublishDate = DateTime.Now.AddDays(-5) }
//     };
//     
//     public IActionResult Index()
//     {
//         return View(_news.OrderByDescending(n => n.PublishDate).ToList());
//     }
//
//     public IActionResult Details(int id)
//     {
//         var newsItem = _news.FirstOrDefault(n => n.Id == id);
//         if (newsItem == null)
//         {
//             return NotFound();
//         }
//
//         return View(newsItem);
//     }
// }

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApp.Controllers
{
    public class NewsController : Controller
    {
        private readonly AppDbContext _context;

        public NewsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: News
        public async Task<IActionResult> Index(int? categoryId, int page = 1)
        {
            int pageSize = 6; // Количество новостей на странице
            
            IQueryable<NewsItem> newsQuery = _context.News
                .Where(n => n.IsPublished)
                .OrderByDescending(n => n.PublishDate);

            // Фильтрация по категории если указана
            if (categoryId.HasValue)
            {
                newsQuery = newsQuery.Where(n => n.CategoryId == categoryId);
            }

            // Пагинация
            var totalItems = await newsQuery.CountAsync();
            var news = await newsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            ViewBag.Categories = await _context.NewsCategories.ToListAsync();
            ViewBag.CurrentCategoryId = categoryId;

            return View(news);
        }

        // GET: News/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsItem = await _context.News
                .Include(n => n.Category) // Если используете категории
                .FirstOrDefaultAsync(m => m.Id == id && m.IsPublished);
                
            if (newsItem == null)
            {
                return NotFound();
            }

            return View(newsItem);
        }

        // GET: News/Create (только для администратора)
        public IActionResult Create()
        {
            ViewBag.Categories = _context.NewsCategories.ToList();
            return View();
        }

        // POST: News/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewsItem newsItem, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                newsItem.PublishDate = DateTime.Now;
                _context.Add(newsItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            ViewBag.Categories = _context.NewsCategories.ToList();
            return View(newsItem);
        }

        // GET: News/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsItem = await _context.News.FindAsync(id);
            if (newsItem == null)
            {
                return NotFound();
            }
            
            ViewBag.Categories = _context.NewsCategories.ToList();
            return View(newsItem);
        }

        // POST: News/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NewsItem newsItem)
        {
            if (id != newsItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(newsItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsItemExists(newsItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            
            ViewBag.Categories = _context.NewsCategories.ToList();
            return View(newsItem);
        }

        // GET: News/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsItem = await _context.News
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (newsItem == null)
            {
                return NotFound();
            }

            return View(newsItem);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var newsItem = await _context.News.FindAsync(id);
            if (newsItem != null)
            {
                _context.News.Remove(newsItem);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool NewsItemExists(int id)
        {
            return _context.News.Any(e => e.Id == id);
        }
    }
}