using FluentValidation;
using GatewayService.ApplicationContract.DTO.Auth;

namespace GatewayService.ApplicationContract.Validations
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Username)
                .NotNull().WithMessage("Username is required.")
                .NotEmpty().WithMessage("Username is required.");

            RuleFor(x => x.Password)
                .NotNull().WithMessage("Password is required.")
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}
