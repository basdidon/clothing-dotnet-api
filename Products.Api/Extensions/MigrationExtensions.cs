using Products.Api.Persistance;

namespace Products.Api.Extensions
{
    public static class SeedDatabaseExtensions
    {
        public static Task SendDatabaseAsync(IApplicationBuilder app)
        {
            var scoped = app.ApplicationServices.CreateScope();
            var context = scoped.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return Task.CompletedTask;
        }
    }
}
