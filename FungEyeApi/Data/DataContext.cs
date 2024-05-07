using FungEyeApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FungEyeApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ConfigureUser(builder.Entity<UserEntity>());
        }

        private void ConfigureUser(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Username).HasMaxLength(100).IsRequired();
            builder.Property(u => u.Password).IsRequired();
            builder.Property(u => u.Email).IsRequired();
            builder.HasIndex(u => u.Username).IsUnique();
            builder.HasIndex(u => u.Email).IsUnique();
        }

        public DbSet<UserEntity> Users { get; set; }

    }
}
