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
                AvaliableSizes = AvailableSizeHelper.GetAvaliableSizes(req.AvailableSizes),
            };

            if (req.Thumbnail != null)
            {
                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                var imagePathname = $"{Guid.NewGuid()}.png";
                var filepath = Path.Combine(uploadDir, imagePathname);

                var imageUrl = Path.Combine("images", imagePathname);


                using var stream = req.Thumbnail.OpenReadStream();
                using var fileStream = File.Create(filepath);
                await stream.CopyToAsync(fileStream,ct);

                product.Thumbnail = new()
                {
                    ImagePath = imageUrl
                };
            }

            // save product to database
            await context.Products.AddAsync(product,ct);
            await context.SaveChangesAsync(ct);
        }
    }


}
