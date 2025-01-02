using FungEyeApi.Data;
using FungEyeApi.Data.Entities;
using FungEyeApi.Enums;
using FungEyeApi.Interfaces;
using FungEyeApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static async Task Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();

        //optionsBuilder.UseSqlite(@"Data Source=C:\Users\konra\Documents\pliki\programowanie\FungEye\FungEyeApi\mydatabase.db"); 
        optionsBuilder.UseSqlServer("CONNECTION_STRING");

        using var context = new DataContext(optionsBuilder.Options);
        var blobStorageService = serviceProvider.GetRequiredService<IBlobStorageService>();

        //context.Database.Migrate();

        //await AddUsersAsync(context);
        await AddFungiImagesAsync(context, blobStorageService, @"C:\Users\Konrad\Documents\pliki\projekty\Mushroom_photos");
    }

    private static async Task AddUsersAsync(DataContext context)
    {
        var users = new[]
        {
            UserEntity.Create(RoleEnum.Admin, "adminuser2", "admin2@example.com", BCrypt.Net.BCrypt.HashPassword("admin123"), DateTime.Parse("1985-01-01"), "Admin", "User", null),
            UserEntity.Create(RoleEnum.User, "testuser11", "testuser12@example.com", BCrypt.Net.BCrypt.HashPassword("password123"), DateTime.Parse("1990-05-15"), "Test11", "User11", null),
            UserEntity.Create(RoleEnum.User, "testuser22", "testuser22@example.com", BCrypt.Net.BCrypt.HashPassword("password123"), DateTime.Parse("1992-08-22"), "Test22", "User22", null)
        };

        await context.Users.AddRangeAsync(users);

        context.SaveChanges();
        Console.WriteLine("Użytkownicy zostali dodani.");

    }


    private static async Task AddFungiImagesAsync(DataContext context, IBlobStorageService blobStorageService, string basePath)
    {
        // Odczytaj wszystkie podfoldery w bazie
        var fungiDirectories = Directory.GetDirectories(basePath);

        foreach (var fungiDir in fungiDirectories)
        {
            // Nazwa folderu to nazwa grzyba
            var fungiName = Path.GetFileName(fungiDir);

            var formatedfungiName = fungiName.Replace("_", " ");

            // Sprawdź, czy grzyb już istnieje w bazie, jeśli nie, stwórz encję
            var fungi = await context.Fungies.FirstOrDefaultAsync(f => f.LatinName == formatedfungiName);

            if (fungi == null) { continue; }

            // Odczytaj wszystkie obrazy w folderze grzyba
            var imageFiles = Directory.GetFiles(fungiDir);

            int i = 0;

            foreach (var imagePath in imageFiles)
            {
                using var fileStream = new FileStream(imagePath, FileMode.Open);

                //get IFormFile from fileStream

                var file = new FormFile(fileStream, 0, fileStream.Length, $"{fungiName}_{i}", Path.GetFileName(imagePath));

                i++;

                // Prześlij obraz do Blob Storage i uzyskaj URL
                var imageUrl = await blobStorageService.UploadFile(file, BlobContainerEnum.Fungies);

                // Stwórz encję obrazu i połącz z grzybem
                var fungiImage = new FungiImageEntity
                {
                    FungiEntityId = fungi.Id,
                    ImageUrl = imageUrl
                };

                await context.FungiesImages.AddAsync(fungiImage);
            }

            // Zapisz wszystkie dodane obrazy dla bieżącego grzyba
            await context.SaveChangesAsync();
        }

        Console.WriteLine("Grzyby i obrazy zostały dodane do bazy danych.");
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        // Build configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Register configuration
        services.AddSingleton<IConfiguration>(configuration);

        // Register your BlobStorageService
        services.AddTransient<IBlobStorageService, BlobStorageService>();

        // Register other services if needed
    }
}
