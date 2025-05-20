using Business.Configurations;
using Business.Models;
using Microsoft.EntityFrameworkCore;

namespace Business.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<DebtRecord> debtrecords { get; set; }
        public DbSet<User> Userss { get; set; }
        public DbSet<SentEmail> SentEmails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<UserProduct> UserProducts { get; set; }
    }
}
