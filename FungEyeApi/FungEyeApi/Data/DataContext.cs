using FungEyeApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

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
            ConfigureFollow(builder.Entity<FollowEntity>());
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

            // Nawigacyjne właściwości do relacji wiele-do-wielu
            builder.HasMany(u => u.Follows)
                   .WithOne(f => f.User)
                   .HasForeignKey(f => f.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureFollow(EntityTypeBuilder<FollowEntity> builder)
        {
            builder.ToTable("Follows");
            builder.HasKey(f => new { f.UserId, f.FollowedUserId });
            builder.HasIndex(f => f.UserId);
            builder.HasIndex(f => f.FollowedUserId);
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<FollowEntity> Follows { get; set; }

    }
}
