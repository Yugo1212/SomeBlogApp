using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TwitterCopyApp.Models;

namespace TwitterCopyApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Tag>().HasIndex(n => n.Name).IsUnique();
            modelBuilder.Entity<ApplicationUser>().HasIndex(n => n.UserName).IsUnique();
            modelBuilder.Entity<ApplicationUser>().HasIndex(n => n.Email).IsUnique();
            modelBuilder.Entity<Comment>().HasOne(c => c.Post)
                                          .WithMany(c => c.Comments)
                                          .HasForeignKey(c => c.PostId)
                                          .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
