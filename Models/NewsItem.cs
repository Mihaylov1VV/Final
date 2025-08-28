// namespace MyWebApp.Models;
//
// public class NewItem
// {
//     public int Id { get; set; }
//     public string Title { get; set; }
//     public string Content { get; set; }
//     public DateTime PublishDate { get; set; }
// }

using System;
using System.ComponentModel.DataAnnotations;

namespace MyWebApp.Models
{
    // public class NewsCategory
    // {
    //     public int Id { get; set; }
    //     
    //     [Required(ErrorMessage = "Название категории обязательно")]
    //     [StringLength(100, ErrorMessage = "Название не может превышать 100 символов")]
    //     public string Name { get; set; }
    //     
    //     public string Description { get; set; }
    //     
    //     // Навигационное свойство
    //     public List<NewsItem> NewsItems { get; set; } = new List<NewsItem>();
    // }
    public class NewsItem
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Заголовок обязателен")]
        [StringLength(200, ErrorMessage = "Заголовок не может превышать 200 символов")]
        public string Title { get; set; }
        
        [Required(ErrorMessage = "Содержание обязательно")]
        public string Content { get; set; }
        
        [DataType(DataType.DateTime)]
        public DateTime PublishDate { get; set; } = DateTime.Now;
        
        public string ImageUrl { get; set; } = "/images/temp/default-news.jpg";
        
        [StringLength(500, ErrorMessage = "Краткое описание не может превышать 500 символов")]
        public string ShortDescription { get; set; }
        
        public bool IsPublished { get; set; } = true;
        
        // Навигационное свойство (если нужно будет добавлять категории)
        public int? CategoryId { get; set; }
        public NewsCategory Category { get; set; }
    }
}