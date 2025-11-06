using AccountService.ApplicationContract.DTO.Permission;
using FluentValidation;

namespace AccountService.ApplicationContract.Validations
{
    public class CreatePermissionDtoValidator : AbstractValidator<PermissionDto>
    {
        public CreatePermissionDtoValidator()
        {
            RuleFor(c => c.Resource)
                .NotNull().WithMessage("Resource is required.")
                .NotEmpty().WithMessage("Resource is required.");

            RuleFor(c => c.Action)
                .NotNull().WithMessage("Action is required.")
                .NotEmpty().WithMessage("Action is required.");
        }
    }
}
