using System.Collections.Generic;

namespace InovaBank.Application.Exceptions.ExceptionsBase
{
    public class ErrorOnValidationException : InovaBankException
    {
        public IList<string> ErrorMessages { get; set; }

        public ErrorOnValidationException(IList<string> errorMessages) : base("")
        {
            ErrorMessages = errorMessages;
        }
    }
}
