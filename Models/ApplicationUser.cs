#nullable enable

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MyWebApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "Имя")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Дата регистрации")]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        [Display(Name = "Аватар")]
        public string? ProfilePicture { get; set; }

        [Display(Name = "О себе")]
        [StringLength(500)]
        public string? Bio { get; set; }

        // Полное имя для отображения
        [Display(Name = "Полное имя")]
        public string FullName => $"{FirstName} {LastName}";
    }
}