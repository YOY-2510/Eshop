namespace EShop.Dto.ProductModel
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal CostPrice { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime DateCreated { get; set; }
        public int StockQuantity { get; set; }
    }
}
