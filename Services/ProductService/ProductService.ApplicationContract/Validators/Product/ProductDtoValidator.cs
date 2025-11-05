using FluentValidation;
using ProductService.ApplicationContract.DTO.Product;

namespace ShopService.ApplicationContract.Validators.Product
{
    public class ProductDtoValidator : AbstractValidator<ProductRequestDto>
    {
        public ProductDtoValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(50);
            RuleFor((c => c.Description))
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(300);
        }
    }
}
