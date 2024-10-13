using FungEyeApi.Data;
using FungEyeApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FungEyeApi.Services
{
    public class DeleteExpiredAccountsService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public DeleteExpiredAccountsService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Wykonaj logikę co 24 godziny (lub inny interwał czasowy)
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);

                // Wywołaj metodę czyszczącą konta
                await DeleteExpiredAccountsAsync();
            }
        }

        private async Task DeleteExpiredAccountsAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                var blobService = scope.ServiceProvider.GetRequiredService<IBlobStorageService>();

                var expiredUsers = await dbContext.Users
                    .Where(u => u.DateDeleted != null && u.DateDeleted <= DateTime.Now.AddDays(-30))
                    .ToListAsync();

                if (expiredUsers.Count > 0)
                {
                    foreach (var user in expiredUsers)
                    {
                        if (!string.IsNullOrEmpty(user.ImageUrl))
                        {
                            await blobService.DeleteFile(user.ImageUrl);
                        }
                    }

                    dbContext.Users.RemoveRange(expiredUsers);
                    await dbContext.SaveChangesAsync();

                    
                }
            }
        }
    }

}
