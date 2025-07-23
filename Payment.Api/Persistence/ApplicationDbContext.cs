using Microsoft.EntityFrameworkCore;
using Payment.Api.Models;

namespace Payment.Api.Persistence
{
    public class ApplicationDbContext(DbContextOptions options):DbContext(options)
    {
        public DbSet<OrderPayment> OrderPayments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
