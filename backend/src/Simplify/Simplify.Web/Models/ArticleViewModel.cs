using System.ComponentModel.DataAnnotations;

namespace Simplify.Web.Models
{
    public class ArticleViewModel
    {
        [Required]
        [StringLength(50000)]
        public string Content { get; set; } = null!;
    }
}