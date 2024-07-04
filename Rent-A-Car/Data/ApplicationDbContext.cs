using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rent_A_Car.Models;

namespace Rent_A_Car.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .HasIndex(x => x.UserName)
                .IsUnique();

            builder.Entity<ApplicationUser>()
              .HasIndex(x => x.Email)
              .IsUnique();

            builder.Entity<ApplicationUser>()
              .HasIndex(x => x.PIN)
              .IsUnique();
        }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Cars> Cars { get; set; }
        public DbSet<Requests> Requests { get; set; }
    }
}