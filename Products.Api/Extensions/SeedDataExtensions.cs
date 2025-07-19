using Microsoft.EntityFrameworkCore;
using Products.Api.Models;
using Products.Api.Persistance;

namespace Products.Api.Extensions
{
    public static class SeedDataExtensions
    {
        public static DbContextOptionsBuilder UseCustomAsyncSeeding(this DbContextOptionsBuilder builder)
        {
            builder.UseAsyncSeeding(async (context, _, ct) =>
            {
                Product[] products = [
                new(){
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    Title = "Demi",
                    SubTitle = "pink floral embroidered stretch cotton corset top" ,
                    UnitPrice = 109,
                    Description = "Lorem ipsum dolor, sit amet consectetur adipisicing elit. Dolorem ad explicabo eaque consectetur, dolore, error nostrum provident autem voluptatum libero ea distinctio veniam eos atque mollitia expedita sequi quo in nobis sapiente minima tempora id sunt? Sint ex corrupti nostrum odio cumque magni tempore similique repudiandae obcaecati?",
                    AvaliableSizes = AvaliableSizes.All
                },
                new(){
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                    Title = "Ysabella",
                    SubTitle = "cream rose print stretch cotton mini sundress",
                    UnitPrice = 179,
                    Description = "Lorem ipsum dolor, sit amet consectetur adipisicing elit. Dolorem ad explicabo eaque consectetur, dolore, error nostrum provident autem voluptatum libero ea distinctio veniam eos atque mollitia expedita sequi quo in nobis sapiente minima tempora id sunt? Sint ex corrupti nostrum odio cumque magni tempore similique repudiandae obcaecati?",
                    AvaliableSizes = AvaliableSizes.All
                }
];
                await context.Set<Product>().AddRangeAsync(products, ct);
                await context.SaveChangesAsync(ct);
            });

            return builder;
        }

        public static async Task ApplyMigrationsAsync(this IApplicationBuilder app, bool resetOnStart = false, CancellationToken ct = default)
        {
            var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (resetOnStart) 
                await context.Database.EnsureDeletedAsync(ct);

            await context.Database.MigrateAsync(ct);
        }
    }
}
