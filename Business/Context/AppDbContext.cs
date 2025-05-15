using Business.Models;
using Microsoft.EntityFrameworkCore;

namespace Business.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<DebtRecord> debtrecords { get; set; }
        public DbSet<User> Userss { get; set; } 
        public DbSet<Register> Registers { get; set; }

    }
}
