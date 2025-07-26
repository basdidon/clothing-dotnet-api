using Contract;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Products.Api.Persistance;

namespace Products.Api.Consumers
{
    public class GetProductsRequestHandler(ApplicationDbContext dbContext) : IConsumer<GetProductsRequest>
    {
        public async Task Consume(ConsumeContext<GetProductsRequest> context)
        {
            var productDtos = await dbContext.Products.AsNoTracking()
                .Where(x => context.Message.ProductIds.Contains(x.Id))
                .Select(x => new ProductDto() { ProductId = x.Id, Title = x.Title, UnitPrice = x.UnitPrice })
                .ToDictionaryAsync(x=> x.ProductId);

            await context.RespondAsync(new GetProductsResponse(productDtos));
        }
    }
}