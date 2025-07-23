using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SharedLibrary.Persistance.Extensions
{
    public static class MigrationExtensions
    {
        public static async Task EnsureDatabaseCreatedAsync<T>(this IApplicationBuilder app, bool resetOnStart = true, CancellationToken ct = default) where T : DbContext
        {
            var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<T>();

            if (resetOnStart)
                await context.Database.EnsureDeletedAsync(ct);

            await context.Database.EnsureCreatedAsync(ct);
        }

        public static async Task ApplyMigrationsAsync<T>(this IApplicationBuilder app, bool resetOnStart = true, CancellationToken ct = default) where T : DbContext
        {
            var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<T>();

            if (resetOnStart)
                await context.Database.EnsureDeletedAsync(ct);

            await context.Database.MigrateAsync(ct);
        }
    }
}
