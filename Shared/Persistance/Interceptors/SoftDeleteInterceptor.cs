using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SharedLibrary.Interfaces;

namespace SharedLibrary.Persistance.Interceptors
{
    /* [ Injection ]
       
        builder.Services.AddSingleton<SoftDeleteInterceptor>();
      
        builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider,options) => {
            var settings = serviceProvider.GetRequiredService<IOptions<DatabaseSettings>>().Value;
            options.UseNpgsql(settings.DefaultConnection)
                .AddInterceptors(serviceProvider.GetRequiredService<SoftDeleteInterceptor>());
        });
      
       [Apply Query Filter] 
        
       protected override void OnModelCreating(ModelBuilder modelBuilder)
       {
           modelBuilder.Entity<Product>()
               .HasQueryFilter(x => !x.IsDeleted);
           
            // OR
    
            modelBuilder.Entity<Product>()
               .HasIndex(x => x.IsDeleted)
               .HasFilter("\"IsDeleted\" = false"); 
            
            // "\"IsDeleted\" = false"      for PostgreSQL
            // "[IsDeleted] = 0"            for sql server
       }
     */
    public sealed class SoftDeleteInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            if (eventData.Context is null)
            {
                return base.SavingChangesAsync(
                    eventData, result, cancellationToken);
            }

            IEnumerable<EntityEntry<ISoftDeletable>> entries =
                eventData
                    .Context
                    .ChangeTracker
                    .Entries<ISoftDeletable>()
                    .Where(e => e.State == EntityState.Deleted);

            foreach (EntityEntry<ISoftDeletable> softDeletable in entries)
            {
                softDeletable.State = EntityState.Modified;
                softDeletable.Entity.IsDeleted = true;
                softDeletable.Entity.DeleteOnUtc = DateTime.UtcNow;
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
