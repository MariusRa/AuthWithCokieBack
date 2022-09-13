using APIAuthentificationUsingJWT.Models;
using Microsoft.EntityFrameworkCore;

namespace APIAuthentificationUsingJWT.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        
        public string DbPath { get; private set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("Data Source=DESKTOP-G9DK1PI; Initial Catalog=APIAuthTask; Integrated security=true;");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
            });
        }
    }
}
