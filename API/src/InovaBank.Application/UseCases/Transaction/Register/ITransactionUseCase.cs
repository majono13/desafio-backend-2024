using InovaBank.Communication.Requests.Transactions;

namespace InovaBank.Application.UseCases.Transaction.Register
{
    public interface ITransactionUseCase
    {
        public Task Deposit(RequestTransactionJson request);
        public Task Withdrawal(RequestTransactionJson request);
        public Task Transfer(RequestTransactionJson request);
    }
}
