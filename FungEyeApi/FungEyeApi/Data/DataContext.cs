using FungEyeApi.Data.Entities;
using FungEyeApi.Data.Entities.Fungies;
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
            ConfigureFollow(builder.Entity<FollowEntity>());
            ConfigurePost(builder.Entity<PostEntity>());
            ConfigureComment(builder.Entity<CommentEntity>());
            ConfigureReaction(builder.Entity<PostReactionEntity>());
            ConfigureFungi(builder.Entity<FungiEntity>());
            ConfigureUserFungi(builder.Entity<UserFungiCollectionEntity>());
            ConfigureReport(builder.Entity<ReportEntity>());
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
                .OnDelete(DeleteBehavior.Cascade);

            post.HasMany(p => p.Reactions)
                .WithOne(r => r.Post)
                .HasForeignKey(r => r.PostId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureComment(EntityTypeBuilder<CommentEntity> comment)
        {
            comment.ToTable("Comments");
            comment.HasKey(c => c.Id);
            comment.Property(c => c.Content).IsRequired();

            comment.HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);

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
                .OnDelete(DeleteBehavior.Cascade);

            reaction.HasOne(r => r.User)
                .WithMany(u => u.Reactions)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureFungi(EntityTypeBuilder<FungiEntity> fungi)
        {
            fungi.ToTable("Fungies");
            fungi.HasKey(f => f.Id);
            fungi.Property(f => f.LatinName).HasMaxLength(255).IsRequired();
            fungi.Property(f => f.PolishName).HasMaxLength(255);
            fungi.Property(f => f.Description).HasMaxLength(-1);
            fungi.Property(f => f.Edibility).HasMaxLength(100);
            fungi.Property(f => f.Toxicity).HasMaxLength(100);
            fungi.Property(f => f.Habitat).HasMaxLength(100);

            fungi.HasMany(f => f.Images)
                 .WithOne(img => img.FungiEntity)
                 .HasForeignKey(img => img.FungiEntityId)
                 .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureUserFungi(EntityTypeBuilder<UserFungiCollectionEntity> collection)
        {
            collection.ToTable("UserFungiCollections");
            collection.HasKey(uf => new { uf.UserId, uf.FungiId });

            collection.HasOne(uf => uf.User)
                   .WithMany(u => u.FungiCollection)
                   .HasForeignKey(uf => uf.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

            collection.HasOne(uf => uf.Fungi)
                   .WithMany(f => f.UserCollections)
                   .HasForeignKey(uf => uf.FungiId)
                   .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureReport(EntityTypeBuilder<ReportEntity> report)
        {
            report.ToTable("Reports");
            report.HasKey(r => r.Id);

            report.HasOne(r => r.ReportedBy)
                .WithMany(u => u.Reports)
                .HasForeignKey(r => r.ReportedById)
                .OnDelete(DeleteBehavior.Restrict);

            report.HasOne(r => r.Post)
                .WithMany(p => p.Reports)
                .HasForeignKey(r => r.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            report.HasOne(r => r.Comment)
                .WithMany(c => c.Reports)
                .HasForeignKey(r => r.CommentId)
                .OnDelete(DeleteBehavior.Restrict);
        }



        public DbSet<UserEntity> Users { get; set; }
        public DbSet<FollowEntity> Follows { get; set; }
        public DbSet<PostEntity> Posts { get; set; }
        public DbSet<CommentEntity> Comments { get; set; }
        public DbSet<PostReactionEntity> Reactions { get; set; }
        public DbSet<FungiEntity> Fungies { get; set; }
        public DbSet<UserFungiCollectionEntity> FungiesUserCollections { get; set; }
        public DbSet<FungiImageEntity> FungiesImages { get; set; }
        public DbSet<ReportEntity> Reports { get; set; }
    }
}
