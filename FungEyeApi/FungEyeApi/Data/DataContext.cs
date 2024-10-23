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
            ConfigureComment(builder.Entity<CommentEntity>());
            ConfigureReaction(builder.Entity<PostReactionEntity>());
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

            user.HasMany(u => u.Follows)
                .WithOne(f => f.User)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            user.HasMany(u => u.Comments)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            user.HasMany(u => u.Reactions)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
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
            post.ToTable("Posts");
            post.HasKey(p => p.Id);
            post.Property(p => p.Content).IsRequired();

            post.HasMany(p => p.Comments)
                .WithOne(c => c.Post)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Restrict);
            
            post.HasMany(p => p.Reactions)
                .WithOne(r => r.Post)
                .HasForeignKey(r => r.PostId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureComment(EntityTypeBuilder<CommentEntity> comment)
        {
            comment.ToTable("Comments");
            comment.HasKey(c => c.Id);
            comment.Property(c => c.Content).IsRequired();

            comment.HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            comment.HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureReaction(EntityTypeBuilder<PostReactionEntity> reaction)
        {
            reaction.ToTable("Reactions");
            reaction.HasKey(r => r.Id);

            reaction.HasOne(r => r.Post)
                .WithMany(p => p.Reactions)
                .HasForeignKey(r => r.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            reaction.HasOne(r => r.User)
                .WithMany(u => u.Reactions)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<FollowEntity> Follows { get; set; }
        public DbSet<PostEntity> Posts { get; set; }
        public DbSet<CommentEntity> Comments { get; set; }
        public DbSet<PostReactionEntity> Reactions { get; set; }
    }
}
