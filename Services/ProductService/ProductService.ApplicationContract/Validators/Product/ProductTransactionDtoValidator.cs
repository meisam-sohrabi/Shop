using FluentValidation;
using ProductService.ApplicationContract.DTO.Transaction;

namespace ProductService.ApplicationContract.Validators.Product
{
    public class ProductTransactionDtoValidator : AbstractValidator<ProductTransactionDto>
    {
        public ProductTransactionDtoValidator()
        {
            RuleFor(c => c.Product)
                .NotNull().WithMessage("Product is required.")
                .SetValidator(new ProductDtoValidator());

            RuleFor(c => c.ProductDetail)
                .NotNull().WithMessage("ProductDetail is required.")
                .SetValidator(new ProductDetailDtoValidator());

            RuleFor(c => c.Price)
                .NotNull().WithMessage("Price is required.")
                .NotEmpty().WithMessage("Price is required.")
                .GreaterThan(0).WithMessage("Price should be postive number.");
        }
    }
}
