using Microsoft.EntityFrameworkCore;
using ProyectoReportes.Models;

namespace ProyectoReportes.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Incident> Incidents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().ToTable("CT_Accounts");
            modelBuilder.Entity<UserProfile>().ToTable("CT_UserProfiles");
            modelBuilder.Entity<Incident>().ToTable("CT_Incidents");

            base.OnModelCreating(modelBuilder);
        }
    }
}
