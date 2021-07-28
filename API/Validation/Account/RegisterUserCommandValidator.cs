using API.Commands;
using FluentValidation;

namespace API.Validation.Account
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(c => c.UserDto.Password)
                .MinimumLength(4)
                .MaximumLength(8)
                .NotEmpty();
        }
    }
}