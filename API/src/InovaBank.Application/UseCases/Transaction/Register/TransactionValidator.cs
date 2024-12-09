using FluentValidation;
using InovaBank.Communication.Requests.Transactions;

namespace InovaBank.Application.UseCases.Transaction.Register
{
    public class TransactionValidator : AbstractValidator<RequestTransactionJson>
    {
        public TransactionValidator()
        {
            RuleFor(transaction => transaction.Value)
                .NotNull().WithMessage(ErrorsMessages.DEPOSIT_VALUE_REQUIRED)
                .GreaterThan(0).WithMessage(ErrorsMessages.DEPOSIT_VALUE_REQUIRED);

            RuleFor(transaction => transaction.Account)
                .NotEmpty().WithMessage(ErrorsMessages.ACCOUNT_REQUIRED)
                .NotNull().WithMessage(ErrorsMessages.ACCOUNT_REQUIRED);

            RuleFor(transaction => transaction.Digit)
             .NotEmpty().WithMessage(ErrorsMessages.DIGIT_REQUIRED)
             .NotNull().WithMessage(ErrorsMessages.DIGIT_REQUIRED);

            RuleFor(transaction => transaction.Agency)
             .NotEmpty().WithMessage(ErrorsMessages.AGENCY_REQUIRED)
             .NotNull().WithMessage(ErrorsMessages.AGENCY_REQUIRED);

        }
    }
}
