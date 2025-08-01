using Api.Constants;
using Api.Persistance;
using Api.Services;

namespace Api.Extensions
{
    public static class SeedDatabaseExtensions
    {
        public static async Task SeedDatabase(this IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var roleService = scope.ServiceProvider.GetRequiredService<RoleService>();
            
            // reset database by delete and create again
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            // create application roles
            await roleService.CreateRolesAsync([Role.Admin, Role.Customer]);
        }
    }
}
