using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using MyWebApp.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Добавляем сервисы в контейнер
builder.Services.AddControllersWithViews();

// Настройка Entity Framework с SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Настройка Identity - БЕЗ .AddDefaultUI()
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Настройка аутентификации cookies
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.LoginPath = "/Account/Login"; // Наши кастомные пути
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = true;
});

var app = builder.Build();

// Конфигурация HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Добавляем аутентификацию и авторизацию
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// УБРАТЬ эту строку, так как мы убрали .AddDefaultUI()
// app.MapRazorPages();

// Создание ролей и администратора при первом запуске
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // Применяем миграции
        context.Database.Migrate();

        // Создаем роли
        await CreateRoles(roleManager);
        
        // Создаем администратора
        await CreateAdminUser(userManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run();

// Метод для создания ролей
static async Task CreateRoles(RoleManager<IdentityRole> roleManager)
{
    string[] roleNames = { "Admin", "Editor", "User" };

    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}

// Метод для создания администратора
static async Task CreateAdminUser(UserManager<ApplicationUser> userManager)
{
    var adminUser = await userManager.FindByEmailAsync("admin@sarapul.ru");
    if (adminUser == null)
    {
        var admin = new ApplicationUser
        {
            UserName = "admin",
            Email = "admin@sarapul.ru",
            FirstName = "Администратор",
            LastName = "Сайта",
            EmailConfirmed = true
        };

        var createAdmin = await userManager.CreateAsync(admin, "Admin123!");
        if (createAdmin.Succeeded)
        {
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}