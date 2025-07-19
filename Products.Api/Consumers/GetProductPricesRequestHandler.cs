using Contract;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Products.Api.Persistance;

namespace Products.Api.Consumers
{
    public class GetProductPricesRequestHandler(ApplicationDbContext dbContext) : IConsumer<GetProductsRequest>
    {
        public Task Consume(ConsumeContext<GetProductsRequest> context)
        {
            var productDtos = dbContext.Products.AsNoTracking()
                .Where(x => context.Message.ProductIds.Contains(x.Id))
                .Select(x => new ProductDto() { ProductId = x.Id, Title = x.Title, UnitPrice = x.UnitPrice })
                .ToDictionaryAsync(x=> x.ProductId);

            return context.RespondAsync(productDtos);
        }
    }
}
