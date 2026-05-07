using ITSupportForum.Data;
using ITSupportForum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ITSupportForum.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Posts
        [Authorize]
        public async Task<IActionResult> Index(string keyword)
        {
            var posts = from p in _context.Post
                        select p;

            if (!string.IsNullOrEmpty(keyword))
            {
                posts = posts.Where(x =>
                    x.Title.Contains(keyword));
            }

            return View(await posts.ToListAsync());
        }

        // GET: Posts/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
    .Include(p => p.Comments)
    .FirstOrDefaultAsync(m => m.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            Post post,
            IFormFile file)
        {
            // AUTO DATE
            post.CreatedAt = DateTime.Now;

            // UPLOAD IMAGE
            if (file != null)
            {
                var fileName = Guid.NewGuid().ToString()
                    + Path.GetExtension(file.FileName);

                var uploadPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/uploads");

                // TẠO FOLDER NẾU CHƯA CÓ
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                post.ImageUrl = fileName;
            }

            // SAVE DATABASE
            post.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _context.Add(post);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Posts/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            var currentUserId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            // ADMIN HOẶC CHỦ BÀI
            if (post.UserId != currentUserId &&
                !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            return View(post);
        }

        // POST: Posts/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            var oldPost = await _context.Post
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (oldPost == null)
            {
                return NotFound();
            }

            var currentUserId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            // ADMIN HOẶC CHỦ BÀI
            if (oldPost.UserId != currentUserId &&
                !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            // GIỮ USERID CŨ
            post.UserId = oldPost.UserId;

            // GIỮ ẢNH CŨ
            post.ImageUrl = oldPost.ImageUrl;

            // GIỮ NGÀY CŨ
            post.CreatedAt = oldPost.CreatedAt;

            try
            {
                _context.Update(post);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(post.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Posts/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .FirstOrDefaultAsync(m => m.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            var currentUserId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            // ADMIN HOẶC CHỦ BÀI
            if (post.UserId != currentUserId &&
                !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Post.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            var currentUserId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            // ADMIN HOẶC CHỦ BÀI
            if (post.UserId != currentUserId &&
                !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            _context.Post.Remove(post);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Post.Any(e => e.Id == id);
        }
        [HttpPost]
        public async Task<IActionResult> AddComment(
    int postId,
    string content)
        {
            var comment = new Comment
            {
                PostId = postId,
                Content = content
            };

            _context.Comment.Add(comment);

            await _context.SaveChangesAsync();

            return RedirectToAction(
                "Details",
                new { id = postId });
        }
    }
}