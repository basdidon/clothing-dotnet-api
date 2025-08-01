using Api.Constants;
using Api.Models;
using Api.Persistance;
using Api.Utilities;
using FastEndpoints;

namespace Api.Endpoints.Products.Create
{
    public class Endpoint(ApplicationDbContext context) : Endpoint<Request>
    {
        public override void Configure()
        {
            Post("products");
            Roles(Role.Admin);
            AllowFileUploads();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            Product product = new()
            {
                Title = req.Title,
                SubTitle = req.SubTitle,
                Description = req.Description,
                UnitPrice = req.UnitPrice,
                AvaliableSizes = AvailableSizeHelper.GetAvaliableSizes(req.AvailableSizes)
            };
            await context.Products.AddAsync(product,ct);
            await context.SaveChangesAsync(ct);
        }
    }


}
