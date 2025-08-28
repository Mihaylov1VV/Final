using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace MyWebApp.Models
{
    public class AppDbContext : DbContext
    {
        // DbSet представляет таблицы в базе данных
        public DbSet<Attraction> Attractions { get; set; }
        public DbSet<NewsItem> News { get; set; }
        public DbSet<CityHistory> CityHistories { get; set; }
        public DbSet<NewsCategory> NewsCategories { get; set; } // Опционально

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // // Конфигурация для NewsItem
            // modelBuilder.Entity<NewsItem>(entity =>
            // {
            //     entity.HasKey(e => e.Id);
            //     entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            //     entity.Property(e => e.Content).IsRequired();
            //     entity.Property(e => e.ShortDescription).HasMaxLength(500);
            //     entity.Property(e => e.ImageUrl).HasDefaultValue("/images/temp/default-news.jpg");
            //     entity.Property(e => e.PublishDate).HasDefaultValueSql("datetime('now')");
            //     entity.Property(e => e.IsPublished).HasDefaultValue(true);
                
            //     // Если используете категории
            //     entity.HasOne(n => n.Category)
            //           .WithMany(c => c.NewsItem)
            //           .HasForeignKey(n => n.CategoryId)
            //           .OnDelete(DeleteBehavior.SetNull);
            // });
            //
            // // Конфигурация для NewsCategory (опционально)
            // modelBuilder.Entity<NewsCategory>(entity =>
            // {
            //     entity.HasKey(e => e.Id);
            //     entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            // });

            // // Добавляем начальные данные для новостей
            // modelBuilder.Entity<NewsItem>().HasData(
            //     new NewsItem 
            //     { 
            //         Id = 1, 
            //         Title = "Фестиваль 'Сарапул - город на Каме'", 
            //         Content = "В предстоящие выходные в нашем городе состоится ежегодный фестиваль 'Сарапул - город на Каме'. В программе: выступления творческих коллективов, ярмарка ремесел, экскурсии по историческим местам и гастрономические мастер-классы. Ждем всех жителей и гостей города!", 
            //         ShortDescription = "Ежегодный городской фестиваль культуры и традиций",
            //         PublishDate = DateTime.Now.AddDays(-2),
            //         ImageUrl = "/images/news/festival-2024.jpg",
            //         IsPublished = true
            //     },
            //     new NewsItem 
            //     { 
            //         Id = 2, 
            //         Title = "Открытие нового парка отдыха", 
            //         Content = "На следующей неделе планируется торжественное открытие нового парка отдыха в центре города. В парке будут установлены современные детские площадки, спортивные тренажеры и зоны для пикника. Это станет прекрасным местом для отдыха горожан.", 
            //         ShortDescription = "Новый зеленый парк для отдыха жителей",
            //         PublishDate = DateTime.Now.AddDays(-5),
            //         ImageUrl = "/images/news/new-park.jpg",
            //         IsPublished = true
            //     },
            //     new NewsItem 
            //     { 
            //         Id = 3, 
            //         Title = "Реконструкция исторического центра", 
            //         Content = "Администрация города объявляет о начале работ по реконструкции исторического центра Сарапула. В планах - восстановление фасадов исторических зданий, благоустройство улиц и создание пешеходных зон.", 
            //         ShortDescription = "Начало масштабной реконструкции центра города",
            //         PublishDate = DateTime.Now.AddDays(-7),
            //         ImageUrl = "/images/news/reconstruction.jpg",
            //         IsPublished = true
            //     }
            // );
            //
            // // Начальные данные для категорий (опционально)
            // modelBuilder.Entity<NewsCategory>().HasData(
            //     new NewsCategory { Id = 1, Name = "События", Description = "Городские мероприятия и события" },
            //     new NewsCategory { Id = 2, Name = "Культура", Description = "Новости культуры и искусства" },
            //     new NewsCategory { Id = 3, Name = "Спорт", Description = "Спортивные события и достижения" },
            //     new NewsCategory { Id = 4, Name = "Экономика", Description = "Экономические новости и развитие" }
            // );
            //
            // // Обновляем новости с категориями (опционально)
            // modelBuilder.Entity<NewsItem>().HasData(
            //     new { Id = 1, CategoryId = 1 }, // Фестиваль → События
            //     new { Id = 2, CategoryId = 1 }, // Парк → События  
            //     new { Id = 3, CategoryId = 4 }  // Реконструкция → Экономика
            // );
        }
    }
}