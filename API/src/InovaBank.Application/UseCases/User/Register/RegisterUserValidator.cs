using System.Linq;
using FluentValidation;
using InovaBank.Communication.Requests;

namespace InovaBank.Application.UseCases.User.Register
{
    internal class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
    {
        public RegisterUserValidator()
        {
            RuleFor(user => user.Email).NotEmpty().WithMessage(ErrorsMessages.EMAIL_EMPTY);
            RuleFor(user => user.Email).EmailAddress().WithMessage(ErrorsMessages.INVALID_EMAIL);
            RuleFor(user => user.Password.Length).GreaterThanOrEqualTo(6).WithMessage(ErrorsMessages.PASSWORD_MIN_LENGTH);
            //RuleFor(user => user.Document)
            //.NotEmpty().WithMessage(ErrorsMessages.CNPJ_EMPTY)
            //.Must(DocumentValid).WithMessage(ErrorsMessages.INVALID_CNPJ);
        }

        private bool DocumentValid(string document)
        {
            document = new string(document.ToCharArray().Where(char.IsDigit).ToArray());
            
            if (document.Length != 14 || document.Distinct().Count() == 1) return false;

            int[] multiplicator1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicator2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempDocument = document.Substring(0, 12);
            int sum = 0;

            for (int i = 0; i < 12; i++)
            {
                sum += int.Parse(tempDocument[i].ToString()) * multiplicator1[i];
            }

            int rest = (sum % 11);
            int firstDigit = rest < 2 ? 0 : 11 - rest;

            tempDocument += firstDigit;
            sum = 0;

            for (int i = 0; i < 13; i++)
            {
                sum += int.Parse(tempDocument[i].ToString()) * multiplicator2[i];
            }

            rest = (sum % 11);
            int secondDigit = rest < 2 ? 0 : 11 - rest;

            return document.EndsWith($"{firstDigit}{secondDigit}");

        }
    }
}
