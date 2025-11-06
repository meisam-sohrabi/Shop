using AccountService.ApplicationContract.DTO.Role;
using FluentValidation;

namespace AccountService.ApplicationContract.Validations
{
    public class RoleDtoValidator : AbstractValidator<RoleDto>
    {
        public RoleDtoValidator()
        {
            RuleFor(c => c.Name)
                .NotNull().WithMessage("Name is required.")
                .NotEmpty().WithMessage("Name is required.");
        }
    }
}
