using FungEyeApi.Data;
using FungEyeApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FungEyeApi.Services
{
    public class FungEyeBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public FungEyeBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Run tasks every 24 hours
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);

                
                await SendRemiderEmailForExpiredAccountsAsync();
                await DeleteExpiredAccountsAsync();
            }
        }

        private async Task DeleteExpiredAccountsAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                var blobService = scope.ServiceProvider.GetRequiredService<IBlobStorageService>();

                var date = DateTime.Now.AddDays(-30);

                var expiredUsers = await dbContext.Users
                    .Where(u => u.DateDeleted != null && u.DateDeleted <= date)
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


                var date = DateTime.Now.AddDays(-2);

                var expiredUsers = await dbContext.Users
                    .Where(u => u.DateDeleted != null && u.DateDeleted <= date)
                    .ToListAsync();

                if (expiredUsers.Count > 0)
                {
                    foreach (var user in expiredUsers)
                    {
                        await emailService.SendEmailAsync(user.Email, Enums.SendEmailOptionsEnum.RemindOfExpiredAccount);
                    }
                }


                //var expiredUsers = await dbContext.Users
                //    .Where(u => u.DateDeleted != null && u.DateDeleted <= now)
                //    .ToListAsync();

                //Console.WriteLine($"Current Time: {now}");
                //foreach (var user in expiredUsers)
                //{
                //    Console.WriteLine($"User DateDeleted: {user.DateDeleted}");
                //}

                //var date = DateTime.UtcNow;
            }
        }
    }

}
