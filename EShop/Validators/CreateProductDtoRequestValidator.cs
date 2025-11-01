using EShop.Dto;
using EShop.Dto.ProductModel;
using FluentValidation;

namespace EShop.Validators
{
    public class CreateProductDtoRequestValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductDtoRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.");

            RuleFor(x => x.CostPrice)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(x => x.SellingPrice)
               .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Category ID is required.");
        }
    }
}
