using FluentValidation;
using InovaBank.Communication.Requests.User;

namespace InovaBank.Application.UseCases.User.Register
{
    internal class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
    {
        public RegisterUserValidator()
        {
            RuleFor(user => user.Email).NotEmpty().WithMessage(ErrorsMessages.EMAIL_EMPTY);
            RuleFor(user => user.Email).EmailAddress().WithMessage(ErrorsMessages.INVALID_EMAIL);

            RuleFor(user => user.Password.Length).GreaterThanOrEqualTo(6).WithMessage(ErrorsMessages.PASSWORD_MIN_LENGTH);
        }

       
    }
}
