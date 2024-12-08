namespace InovaBank.Application.Exceptions.ExceptionsBase
{
    public class AccountNotFoundException : InovaBankException
    {
        public AccountNotFoundException() : base(ErrorsMessages.ACCOUNT_NOT_FOUND)
        {
        }
    }
}
