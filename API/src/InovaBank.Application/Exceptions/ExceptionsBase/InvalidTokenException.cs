namespace InovaBank.Application.Exceptions.ExceptionsBase
{
    public class InvalidTokenException : InovaBankException
    {
        public InvalidTokenException() : base(ErrorsMessages.IVALID_TOKEN)
        {
        }
    }
}
