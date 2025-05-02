using Login.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Login.API.Data
{
    public class LoginContext : DbContext
    {
        public DbSet<UserClass> TblUser { get; set; }

        public LoginContext(DbContextOptions<LoginContext> options): base(options) 
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserClass>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<UserClass>().HasIndex(u => u.Username).IsUnique();
        }
    }
}
