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
                // Run tasks every 24 hours
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);

                await DeleteExpiredAccountsAsync();
                await SendRemiderEmailForExpiredAccountsAsync();
            }
        }

        private async Task DeleteExpiredAccountsAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                var blobService = scope.ServiceProvider.GetRequiredService<IBlobStorageService>();

                var expiredUsers = await dbContext.Users
                    .Where(u => u.DateDeleted != null && u.DateDeleted <= DateTime.Now.AddMinutes(-5))
                    .ToListAsync();

                if (expiredUsers.Count > 0)
                {
                    foreach (var user in expiredUsers)
                    {
                        if (!string.IsNullOrEmpty(user.ImageUrl))
                        {
                            await blobService.DeleteFile(user.ImageUrl, Enums.BlobContainerEnum.Users);
                        }
                    }

                    dbContext.Users.RemoveRange(expiredUsers);
                    await dbContext.SaveChangesAsync();

                    
                }
            }
        }

        private async Task SendRemiderEmailForExpiredAccountsAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                var expiredUsers = await dbContext.Users
                    .Where(u => u.DateDeleted != null && u.DateDeleted <= DateTime.Now.AddMinutes(-1))
                    .ToListAsync();

                if (expiredUsers.Count > 0)
                {
                    foreach (var user in expiredUsers)
                    {
                        await emailService.SendEmailAsync(user.Email, Enums.SendEmailOptionsEnum.RemindOfExpiredAccount);
                    }
                }
            }
        }
    }

}
