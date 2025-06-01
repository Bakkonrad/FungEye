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
                await DeleteCompletedReportsAsync();
            }
        }

        private async Task DeleteExpiredAccountsAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                var blobService = scope.ServiceProvider.GetRequiredService<IBlobStorageService>();

                var executionStrategy = dbContext.Database.CreateExecutionStrategy();

                await executionStrategy.ExecuteAsync(async () =>
                {
                    await using var transaction = await dbContext.Database.BeginTransactionAsync();

                    try
                    {
                        var date = DateTime.Now.AddDays(-30);

                        var expiredUsers = await dbContext.Users
                            .Where(u => u.DateDeleted != null && u.DateDeleted <= date)
                            .ToListAsync();

                        if (expiredUsers.Count == 0) return; // no users to delete

                        foreach (var user in expiredUsers)
                        {
                            var tasks = new List<Task>();

                            if (!string.IsNullOrWhiteSpace(user.ImageUrl) && !user.ImageUrl.Equals("placeholder"))
                            {
                                tasks.Add(blobService.DeleteFile(user.ImageUrl, Enums.BlobContainerEnum.Users));
                            }

                            var postsWithImages = await dbContext.Posts
                                .Where(p => p.UserId == user.Id && !string.IsNullOrWhiteSpace(p.ImageUrl))
                                .ToListAsync();

                            foreach (var post in postsWithImages)
                            {
                                await blobService.DeleteFile(post.ImageUrl!, Enums.BlobContainerEnum.Posts);
                            }

                            dbContext.Reports.RemoveRange(dbContext.Reports.Where(r => r.ReportedById == user.Id));
                            dbContext.Comments.RemoveRange(dbContext.Comments.Where(c => c.UserId == user.Id));
                            dbContext.Reactions.RemoveRange(dbContext.Reactions.Where(r => r.UserId == user.Id));
                            dbContext.Posts.RemoveRange(dbContext.Posts.Where(p => p.UserId == user.Id));
                            dbContext.FungiesUserCollections.RemoveRange(dbContext.FungiesUserCollections.Where(f => f.UserId == user.Id));
                            dbContext.Follows.RemoveRange(dbContext.Follows.Where(f => f.UserId == user.Id));

                            await Task.WhenAll(tasks);
                        }

                        dbContext.Users.RemoveRange(expiredUsers);
                        await dbContext.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                });
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
            }
        }


        private async Task DeleteCompletedReportsAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

                var date = DateTime.Now.AddHours(-1);

                var completedReports = await dbContext.Reports
                    .Where(u => u.Completed == true && u.ModifiedAt <= date)
                    .ToListAsync();

                if (completedReports.Count > 0)
                {
                    dbContext.RemoveRange(completedReports);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }

}
