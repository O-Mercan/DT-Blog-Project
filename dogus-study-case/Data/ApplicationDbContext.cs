using dogus_study_case.Models; 
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace dogus_study_case.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }

        public DbSet<Comment> Comments { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); 

            // Category - BlogPost ilişkisi (One-to-Many)
            builder.Entity<Category>()
                .HasMany(c => c.BlogPosts)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); 

            // User (IdentityUser) - BlogPost ilişkisi (One-to-Many)
            builder.Entity<IdentityUser>()
                .HasMany<BlogPost>() 
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict); 

            // BlogPost - Comment ilişkisi (One-to-Many)
            builder.Entity<BlogPost>()
                .HasMany(bp => bp.Comments)       
                .WithOne(c => c.BlogPost)         
                .HasForeignKey(c => c.BlogPostId) 
                .OnDelete(DeleteBehavior.Cascade); 

            // User (IdentityUser) - Comment ilişkisi (One-to-Many)
            builder.Entity<IdentityUser>()
                .HasMany<Comment>()              
                .WithOne(c => c.User)           
                .HasForeignKey(c => c.UserId)   
                .OnDelete(DeleteBehavior.Restrict); 

            // Mevcut Seed Data 
            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Teknoloji" },
                new Category { Id = 2, Name = "Yazılım Geliştirme" },
                new Category { Id = 3, Name = "Güncel" },
                new Category { Id = 4, Name = "Yaşam" }
            );
        }
    }
}