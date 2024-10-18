using FungEyeApi.Data.Entities;
using FungEyeApi.Data.Entities.Posts;
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
            ConfigurePost(builder.Entity<PostEntity>());
        }

        private void ConfigureUser(EntityTypeBuilder<UserEntity> user)
        {
            user.ToTable("Users");
            user.HasKey(u => u.Id);
            user.Property(u => u.Username).HasMaxLength(100).IsRequired();
            user.Property(u => u.Password).IsRequired();
            user.Property(u => u.Email).IsRequired();
            user.HasIndex(u => u.Username).IsUnique();
            user.HasIndex(u => u.Email).IsUnique();

            // Nawigacyjne właściwości do relacji wiele-do-wielu
            user.HasMany(u => u.Follows)
                   .WithOne(f => f.User)
                   .HasForeignKey(f => f.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureFollow(EntityTypeBuilder<FollowEntity> follow)
        {
            follow.ToTable("Follows");
            follow.HasKey(f => new { f.UserId, f.FollowedUserId });
            follow.HasIndex(f => f.UserId);
            follow.HasIndex(f => f.FollowedUserId);
        }

        private void ConfigurePost(EntityTypeBuilder<PostEntity> post)
        {
            post.HasMany(p => p.Comments)
                   .WithOne(c => c.Post)
                   .HasForeignKey(c => c.PostId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Configure cascade delete for Reactions
            post.HasMany(p => p.Reactions)
                   .WithOne(r => r.Post)
                   .HasForeignKey(r => r.PostId)
                   .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<FollowEntity> Follows { get; set; }
        public DbSet<PostEntity> Posts { get; set; }
        public DbSet<CommentEntity> Comments { get; set; }
        public DbSet<PostReactionEntity> PostReactions { get; set; }

    }
}
