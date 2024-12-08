using FileTypeChecker.Extensions;
using FluentValidation;
using InovaBank.Application.Exceptions.ExceptionsBase;
using InovaBank.Communication.Requests.Account;
using Microsoft.AspNetCore.Http;

namespace InovaBank.Application.UseCases.Account.Register
{
    public class RegisterAccountValidator : AbstractValidator<RequesteRegisterAccountJson>
    {
        public RegisterAccountValidator()
        {
            RuleFor(account => account.Cnpj)
                .NotEmpty().WithMessage(ErrorsMessages.CNPJ_EMPTY)
                .NotNull().WithMessage(ErrorsMessages.CNPJ_EMPTY)
                .Must(CnpjValid).WithMessage(ErrorsMessages.INVALID_CNPJ);

            RuleFor(account => account.Document)
                .NotEmpty().WithMessage(ErrorsMessages.DOCUMENT_EMPTY)
                .NotNull().WithMessage(ErrorsMessages.DOCUMENT_EMPTY)
                .Must(DocumentValid).WithMessage(ErrorsMessages.INVALID_DOCUMENT);
        }

        private bool CnpjValid(string document)
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

            int rest = sum % 11;
            int firstDigit = rest < 2 ? 0 : 11 - rest;

            tempDocument += firstDigit;
            sum = 0;

            for (int i = 0; i < 13; i++)
            {
                sum += int.Parse(tempDocument[i].ToString()) * multiplicator2[i];
            }

            rest = sum % 11;
            int secondDigit = rest < 2 ? 0 : 11 - rest;

            return document.EndsWith($"{firstDigit}{secondDigit}");

        }

        private bool DocumentValid(IFormFile file)
        {
            var fileStream = file.OpenReadStream();

            return fileStream.IsImage();
        }
    }
}
