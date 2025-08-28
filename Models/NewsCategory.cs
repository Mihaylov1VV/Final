using System.ComponentModel.DataAnnotations;

namespace MyWebApp.Models;
public partial class NewsCategory
{
    public int Id { get; set; }
        
    [Required(ErrorMessage = "Название категории обязательно")]
    [StringLength(100, ErrorMessage = "Название не может превышать 100 символов")]
    public string Name { get; set; }
        
    public string Description { get; set; }
        
    // Навигационное свойство
    public List<NewsItem> NewsItem { get; set; } = new List<NewsItem>();
}