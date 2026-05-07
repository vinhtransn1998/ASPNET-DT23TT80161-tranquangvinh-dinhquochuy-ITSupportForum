using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ITSupportForum.Data;

namespace ITSupportForum.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _context.Post
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return View(posts);
        }
    }
}