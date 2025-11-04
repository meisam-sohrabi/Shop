using FluentValidation;
using ProductService.ApplicationContract.DTO.Category;

namespace ProductService.ApplicationContract.Validators.Category
{
    public class CategoryDtoValidator : AbstractValidator<CategoryDto>
    {
        public CategoryDtoValidator()
        {
            RuleFor(c => c.Name)
                .NotNull().WithMessage("Name can not be null.")
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(50).WithMessage("Maximum length is 50.");

        }
    }
}
