using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Identity;

namespace WebApplication1.Models
{
    /*public class WishListContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<UserClaim> UserClaim { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleClaim> RoleClaim { get; set; }
        public DbSet<WishList> WishList { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<CustomerBooks> Customers { get; set; }
        public WishListContext(DbContextOptions<WishListContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().ToTable("User");
            builder.Entity<UserClaim>().ToTable("UserClaim");
            builder.Entity<UserRole>().ToTable("UserRole").HasKey("UserId", "RoleId");
            builder.Entity<Role>().ToTable("Role");
            builder.Entity<RoleClaim>().ToTable("RoleClaim");
            builder.Entity<Book>().ToTable("Books");
            builder.Entity<CustomerBooks>().ToTable("CustomerBooks");
            builder.Entity<WishList>().ToTable("WishList");
        }
    }*/
}
