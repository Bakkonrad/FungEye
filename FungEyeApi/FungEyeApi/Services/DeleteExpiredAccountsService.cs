using FungEyeApi.Data;
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

                var expiredUsers = await dbContext.Users
                    .Where(u => u.DateDeleted != null && u.DateDeleted <= DateTime.Now.AddDays(-30))
                    .ToListAsync();

                if (expiredUsers.Any())
                {
                    dbContext.Users.RemoveRange(expiredUsers);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }

}
