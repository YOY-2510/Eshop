using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Data
{
   // [Table("Products")]
    public class Product : BaseEntity
    {
        [Required,MinLength(3), MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(10.00,double.MaxValue)]
        public decimal SellingPrice { get; set; }

        [Range(10.00,double.MaxValue)]
        public decimal CostPrice { get; set; }

        [Required,MinLength(3), MaxLength(100)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [ForeignKey("Category")]
        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }

        public DateTime? ExpiryDate { get; set; } = DateTime.UtcNow;

        [Range(0,int.MaxValue)]
        public int StockQuantity { get; set; }
    }
}
