using InovaBank.Application.Exceptions.ExceptionsBase;
using InovaBank.Domain.Repositories.Account;
using InovaBank.Domain.Services.ReceitaWS;

namespace InovaBank.Application.Services.Account
{
    public class CnpjValidate
    {
        private readonly IReceitaWS _receitaWS;

        public CnpjValidate(IReceitaWS receitaWS)
        {
            _receitaWS = receitaWS;
        }
        public async Task<ReceitaWsResponse?> Validate(string cnpj, FluentValidation.Results.ValidationResult validatorResult, IAccountReadOnlyRepository repository)
        {
            var existis = await repository.ExistsAccountWithCnpj(cnpj);

            if (existis)
            {
                validatorResult.Errors.Add(new FluentValidation.Results.ValidationFailure("", ErrorsMessages.CNPJ_REGISTERED));
            }

            try
            {
                var cnpjInfo = await _receitaWS.GetCnpjInfo(cnpj);
                if (cnpjInfo == null || cnpjInfo.Name == null)
                {
                    validatorResult.Errors.Add(new FluentValidation.Results.ValidationFailure("", ErrorsMessages.CNPJ_NOT_FOUND));
                }
                else
                {
                    return cnpjInfo;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Erro 429"))
                {
                    throw new ReceitaWSException();
                }
            }

            return null;
        }
    }
}
