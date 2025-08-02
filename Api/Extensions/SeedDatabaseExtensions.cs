using Api.Constants;
using Api.Models;
using Api.Persistance;
using Api.Services;
using Microsoft.AspNetCore.Identity;

namespace Api.Extensions
{
    public static class SeedDatabaseExtensions
    {
        public static async Task SeedDatabase(this IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var roleService = scope.ServiceProvider.GetRequiredService<RoleService>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // reset database by delete and create again
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            // create application roles
            await roleService.CreateRolesAsync([Role.Admin, Role.Customer]);

            ApplicationUser admin = new()
            {
                Email = "admin@fakemail.com",
                UserName = "Admin",
                EmailConfirmed = true,
            };
            await userManager.CreateAsync(admin, "admin123");
            await context.SaveChangesAsync();

            await userManager.AddToRoleAsync(admin,Role.Admin);
            await context.SaveChangesAsync();

            Product product1 = new()
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Title = "Demi",
                SubTitle = "pink floral embroidered stretch cotton corset top",
                UnitPrice = 109,
                Description = "Lorem ipsum dolor, sit amet consectetur adipisicing elit. Dolorem ad explicabo eaque consectetur, dolore, error nostrum provident autem voluptatum libero ea distinctio veniam eos atque mollitia expedita sequi quo in nobis sapiente minima tempora id sunt? Sint ex corrupti nostrum odio cumque magni tempore similique repudiandae obcaecati?",
                AvaliableSizes = AvaliableSizes.All
            };

            Product product2 = new()
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                Title = "Ysabella",
                SubTitle = "cream rose print stretch cotton mini sundress",
                UnitPrice = 179,
                Description = "Lorem ipsum dolor, sit amet consectetur adipisicing elit. Dolorem ad explicabo eaque consectetur, dolore, error nostrum provident autem voluptatum libero ea distinctio veniam eos atque mollitia expedita sequi quo in nobis sapiente minima tempora id sunt? Sint ex corrupti nostrum odio cumque magni tempore similique repudiandae obcaecati?",
                AvaliableSizes = AvaliableSizes.All
            };

            await context.Products.AddAsync(product1);
            await context.Products.AddAsync(product2);
            await context.SaveChangesAsync();
        }
    }
}
