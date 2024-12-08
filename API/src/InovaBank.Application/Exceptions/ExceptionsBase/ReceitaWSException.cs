namespace InovaBank.Application.Exceptions.ExceptionsBase
{
    public class ReceitaWSException : InovaBankException
    {
        public ReceitaWSException() : base(ErrorsMessages.RECEITAWS_REQUISITION_LIMITE_REACHED)
        {
            
        }
    }
}
