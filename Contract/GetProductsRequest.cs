namespace Contract
{
    public record GetProductsRequest(IEnumerable<Guid> ProductIds);
    public record GetProductsResponse(Dictionary<Guid, ProductDto> Products);
    public record ProductDto
    {
        public Guid ProductId { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
    }
}