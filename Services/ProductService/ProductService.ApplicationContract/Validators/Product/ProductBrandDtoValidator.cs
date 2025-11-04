using FluentValidation;
using ProductService.ApplicationContract.DTO.ProductBrand;

namespace ProductService.ApplicationContract.Validators.Product
{
    public class ProductBrandDtoValidator : AbstractValidator<ProductBrandDto>
    {
        public ProductBrandDtoValidator()
        {
            RuleFor(c => c.Name)
                .NotNull().WithMessage("Brand Name is required.")
                .NotEmpty().WithMessage("Brand Name is required.")
                .MaximumLength(30)
                .WithMessage("Brand Name should be 30 length long.");

            RuleFor(c => c.Description)
                .NotNull().WithMessage("Brand Description is required.")
                .NotEmpty().WithMessage("Brand Description is required.")
                .MaximumLength(350)
                .WithMessage("Brand Description should be 350 length long.");
        }
    }
}
