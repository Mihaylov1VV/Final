using Microsoft.AspNetCore.Authorization;
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

        // GET: News - доступно всем
        public async Task<IActionResult> Index()
        {
            return View(await _context.News.Where(n => n.IsPublished).ToListAsync());
        }

        // GET: News/Details/5 - доступно всем
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsItem = await _context.News
                .FirstOrDefaultAsync(m => m.Id == id && m.IsPublished);
                
            if (newsItem == null)
            {
                return NotFound();
            }

            return View(newsItem);
        }

        // GET: News/Create - только для редакторов и администраторов
        [Authorize(Roles = "Admin,Editor")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: News/Create - только для редакторов и администраторов
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Create(NewsItem newsItem)
        {
            if (ModelState.IsValid)
            {
                newsItem.PublishDate = DateTime.Now;
                _context.Add(newsItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(newsItem);
        }

        // GET: News/Edit/5 - только для редакторов и администраторов
        [Authorize(Roles = "Admin,Editor")]
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
            return View(newsItem);
        }

        // POST: News/Edit/5 - только для редакторов и администраторов
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Editor")]
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
            return View(newsItem);
        }

        // GET: News/Delete/5 - только для администраторов
        [Authorize(Roles = "Admin")]
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

        // POST: News/Delete/5 - только для администраторов
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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