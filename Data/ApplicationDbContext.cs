using ITSupportForum.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ITSupportForum.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
    public DbSet<ITSupportForum.Models.Post> Post { get; set; } = default!;
        public DbSet<Comment> Comment { get; set; }
    }
}
