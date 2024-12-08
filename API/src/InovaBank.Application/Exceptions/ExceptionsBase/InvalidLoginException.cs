namespace InovaBank.Application.Exceptions.ExceptionsBase
{
    public class InvalidLoginException : InovaBankException
    {
        public InvalidLoginException() : base(ErrorsMessages.EMAIL_OR_PASSWORD_INVALID)
        {
            
        }
    }
}
