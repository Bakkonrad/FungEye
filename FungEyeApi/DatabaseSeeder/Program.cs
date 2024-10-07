using FungEyeApi.Data;
using FungEyeApi.Data.Entities;
using FungEyeApi.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

class Program
{
    static async Task Main(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        optionsBuilder.UseSqlite(@"Data Source=C:\Users\konra\Documents\pliki\programowanie\FungEye\FungEyeApi\mydatabase.db");

        using var context = new DataContext(optionsBuilder.Options);

        context.Database.Migrate();

        // Dodaj użytkowników
        await AddUsersAsync(context);

        Console.WriteLine("Użytkownicy zostali dodani.");
    }

    private static async Task AddUsersAsync(DataContext context)
    {
        // Przykładowi użytkownicy do dodania
        var users = new[]
        {
            UserEntity.Create(RoleEnum.Admin, "adminuser", "admin@example.com", BCrypt.Net.BCrypt.HashPassword("admin123"), DateTime.Parse("1985-01-01"), "Admin", "User", null),
            UserEntity.Create(RoleEnum.User, "testuser1", "testuser1@example.com", BCrypt.Net.BCrypt.HashPassword("password123"), DateTime.Parse("1990-05-15"), "Test", "User", null),
            UserEntity.Create(RoleEnum.User, "testuser2", "testuser2@example.com", BCrypt.Net.BCrypt.HashPassword("password123"), DateTime.Parse("1992-08-22"), "Test", "User2", null)
        };

        // Dodanie użytkowników do kontekstu
        await context.Users.AddRangeAsync(users);

        // Zapisanie zmian w bazie danych
        context.SaveChanges();
    }
}
