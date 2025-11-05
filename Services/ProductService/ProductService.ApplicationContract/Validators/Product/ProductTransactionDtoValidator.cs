using FluentValidation;
using ProductService.ApplicationContract.DTO.Transaction;

namespace ProductService.ApplicationContract.Validators.Product
{
    public class ProductTransactionDtoValidator : AbstractValidator<ProductTransactionServiceDto>
    {
        public ProductTransactionDtoValidator()
        {
            RuleFor(c => c.ProductName)
                      .NotEmpty().WithMessage("Name is required.")
                      .MaximumLength(50);
            RuleFor(c => c.ProductDescription)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(300);

            RuleFor(c => c.Size)
                          .NotNull().WithMessage("Size is Required.")
                          .NotEmpty().WithMessage("Size is Required.")
                          .MaximumLength(30).WithMessage("Size must be at most 30 characters long.");

            RuleFor(c => c.DetailDescription)
                .NotNull().WithMessage("Description is Required.")
                .NotEmpty().WithMessage("Description is Required.")
                .MaximumLength(350).WithMessage("Description must be at most 350 characters long.");

            RuleFor(c => c.Price)
                .NotNull().WithMessage("Price is required.")
                .NotEmpty().WithMessage("Price is required.")
                .GreaterThan(0).WithMessage("Price should be postive number.");
        }
    }
}
