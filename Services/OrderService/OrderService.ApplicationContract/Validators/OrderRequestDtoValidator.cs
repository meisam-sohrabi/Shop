using FluentValidation;
using OrderService.ApplicationContract.DTO.Order;

namespace OrderService.ApplicationContract.Validators
{
    public class OrderRequestDtoValidator : AbstractValidator<OrderRequestDto>
    {
        public OrderRequestDtoValidator()
        {
            RuleFor(c => c.Quantity)
                .NotNull().WithMessage("Product Detail is required.")
                .NotEmpty().WithMessage("Product Detail is required.");

            RuleFor(c => c.ProductDetailId)
                .NotNull().WithMessage("Product Detail is required.")
                .NotEmpty().WithMessage("Product Detail is required.");
        }
    }
}
