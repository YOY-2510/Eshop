using System.ComponentModel.DataAnnotations;

namespace EShop.Data
{
    public class Category : BaseEntity
    {
        [Required,MinLength(3),MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required,MinLength(5),MaxLength(100)]
        public string Description { get; set; } = string.Empty;
    }
}
