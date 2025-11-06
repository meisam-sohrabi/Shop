using AccountService.ApplicationContract.DTO.Account;
using FluentValidation;

namespace AccountService.ApplicationContract.Validations
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(c => c.FullName)
                .NotNull().WithMessage("FullName is required.")
                .NotEmpty().WithMessage("FullName is required.");

            RuleFor(c => c.Email)
                .NotNull().WithMessage("Email is required.")
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress();

            RuleFor(c => c.PhoneNumber)
                .NotNull().WithMessage("PhoneNumber is required.")
                .NotEmpty().WithMessage("PhoneNumber is required.");

            RuleFor(c => c.Password)
                .NotNull().WithMessage("Password is required.")
                .NotEmpty().WithMessage("Password is required.");

        }
    }
}
