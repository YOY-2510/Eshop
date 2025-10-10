namespace EShop.Dto.ProductModel
{
    public class CreateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
        public decimal CostPrice { get; set; }
        public int StockQuantiy { get; set; }
        public decimal SellingPrice {get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}
