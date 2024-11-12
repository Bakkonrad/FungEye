using FungEyeApi.Data;
using FungEyeApi.Data.Entities;
using FungEyeApi.Enums;
using Microsoft.EntityFrameworkCore;

class Program
{
    static async Task Main(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        optionsBuilder.UseSqlite(@"Data Source=C:\Users\konra\Documents\pliki\programowanie\FungEye\FungEyeApi\mydatabase.db");

        using var context = new DataContext(optionsBuilder.Options);

        context.Database.Migrate();
        //Zadania do wykonania

        await AddUsersAsync(context);
        //await AddFungiImagesAsync(context);
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


    //private static async Task AddFungiImagesAsync(DataContext context, IBlobStorageService blobStorageService, string basePath)
    //{
    //    // Odczytaj wszystkie podfoldery w bazie
    //    var fungiDirectories = Directory.GetDirectories(basePath);

    //    foreach (var fungiDir in fungiDirectories)
    //    {
    //        // Nazwa folderu to nazwa grzyba
    //        var fungiName = Path.GetFileName(fungiDir);

    //        // Sprawdź, czy grzyb już istnieje w bazie, jeśli nie, stwórz encję
    //        var fungi = await context.Fungies.FirstOrDefaultAsync(f => f.Name == fungiName);
    //        if (fungi == null)
    //        {
    //            fungi = new FungiEntity { Name = fungiName };
    //            await context.Fungies.AddAsync(fungi);
    //            await context.SaveChangesAsync(); // Upewnij się, że grzyb jest zapisany przed dodaniem obrazów
    //        }

    //        // Odczytaj wszystkie obrazy w folderze grzyba
    //        var imageFiles = Directory.GetFiles(fungiDir);

    //        foreach (var imagePath in imageFiles)
    //        {
    //            using var fileStream = new FileStream(imagePath, FileMode.Open);
    //            var fileName = Path.GetFileName(imagePath);

    //            // Prześlij obraz do Blob Storage i uzyskaj URL
    //            var imageUrl = await blobStorageService.UploadFile(fileStream, BlobContainerEnum.Fungi, fileName);

    //            // Stwórz encję obrazu i połącz z grzybem
    //            var fungiImage = new FungiImageEntity
    //            {
    //                FungiId = fungi.Id,
    //                ImageUrl = imageUrl
    //            };

    //            await context.FungiImages.AddAsync(fungiImage);
    //        }

    //        // Zapisz wszystkie dodane obrazy dla bieżącego grzyba
    //        await context.SaveChangesAsync();
    //    }

    //    Console.WriteLine("Grzyby i obrazy zostały dodane do bazy danych.");
    //}

}
