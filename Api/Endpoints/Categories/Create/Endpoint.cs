using Api.Constants;
using Api.Endpoints.Categories.List;
using Api.Models;
using Api.Persistance;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints.Categories.Create
{

    public class Endpoint(ApplicationDbContext context) : Endpoint<Request,CategoryDto>
    {
        public override void Configure()
        {
            Post("categories");            
            Roles(Role.Admin);
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var existsCategory = await context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Name == req.Name, ct);

            if(existsCategory != null)
            {
                AddError(x=>x.Name,$"Category with name : {req.Name} already exists.");
                await Send.ErrorsAsync(409,ct);
                return;
            }

            Category category = new()
            {
                Name = req.Name,
            };


            await context.Categories.AddAsync(category,ct);
            await context.SaveChangesAsync(ct);

            Response = new()
            {
                Id = category.Id,
                Name = category.Name
            };
        }
    }
}
