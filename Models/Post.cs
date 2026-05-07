using System.ComponentModel.DataAnnotations;

namespace ITSupportForum.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Content { get; set; }

        public string ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? UserId { get; set; }
        public ICollection<Comment>? Comments { get; set; }
    }
}
