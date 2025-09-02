using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;

namespace MyWebApp.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Attraction> Attractions { get; set; }
        public DbSet<NewsItem> News { get; set; }
        public DbSet<CityHistory> CityHistories { get; set; }
        
        public DbSet<NewsCategory> NewsCategories {get; set; } 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Важно: сначала базовый метод

            // Ваша существующая конфигурация...
            modelBuilder.Entity<NewsItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Content).IsRequired();
                entity.Property(e => e.ShortDescription).HasMaxLength(500);
                entity.Property(e => e.ImageUrl).HasDefaultValue("/images/temp/default-news.jpg");
                entity.Property(e => e.PublishDate).HasDefaultValueSql("datetime('now')");
                entity.Property(e => e.IsPublished).HasDefaultValue(true);
            });

            // Можно добавить начальные данные для ролей и администратора
        }
    }
}