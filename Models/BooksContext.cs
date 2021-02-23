using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WebApplication1.Identity;

namespace WebApplication1.Models
{
    public class BooksContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<UserClaim> UserClaim { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleClaim> RoleClaim { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<CustomerBooks> Customers { get; set; }
        public DbSet<Followers> Followers { get; set; }
        public DbSet<MessageModel> OfflineMessages { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<Purchased> Purchased { get; set; }
        public BooksContext(DbContextOptions<BooksContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().ToTable("User").Ignore(x => x.IsOnline).Ignore(x => x.FollowType);
            builder.Entity<UserClaim>().ToTable("UserClaim");
            builder.Entity<UserRole>().ToTable("UserRole").HasKey("UserId", "RoleId");
            builder.Entity<Role>().ToTable("Role");
            builder.Entity<RoleClaim>().ToTable("RoleClaim");
            builder.Entity<Book>().ToTable("Books");
            builder.Entity<CustomerBooks>().ToTable("CustomerBooks");
            builder.Entity<Followers>().ToTable("Followers").HasKey(f => new { f.UserId, f.FollowUserId });
            builder.Entity<WishList>().ToTable("WishList").HasKey(f => new { f.CustomerId, f.BookId});
            builder.Entity<Purchased>().ToTable("Purchased").HasKey(f => new { f.CustomerId, f.BookId});
            builder.Entity<MessageModel>().ToTable("OfflineMessages").Ignore(x => x.IsRead);
        }
    }
}
