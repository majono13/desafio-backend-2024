using InovaBank.Application.UseCases.Account.Get;
using InovaBank.Application.UseCases.Account.Register;
using InovaBank.Application.UseCases.Account.Update;
using InovaBank.Communication.Requests.Account;
using InovaBank.Communication.Requests.Transactions;
using InovaBank.Communication.Responses.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InovaBank.API.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisterAccountJson), StatusCodes.Status201Created)]
        public async Task<IActionResult> Register(
            [FromServices] IRegisterAccountUseCase useCase,
            [FromForm] RequesteRegisterAccountJson request
            )
        {
            var result = await useCase.Execute(request);

            return Created("", result);
        }

        [HttpPut]
        [ProducesResponseType(typeof(ResponseRegisterAccountJson), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(
            [FromServices] IUpdateAccountUseCase useCase,
            [FromForm] RequesteRegisterAccountJson request
            )
        {
            var result = await useCase.Execute(request);

            return Ok(result);

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(
            [FromServices] IUpdateAccountUseCase useCase,
            [FromBody] RequestAccountJson request)
        {
            await useCase.MarkAsDeleted(request);
            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponseAccountJson), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAccount(
            [FromServices] IGetAccountUseCase useCase,
            [FromQuery] string accountNumber)
        {
            var request = new RequestAccountJson { AccountNumber = accountNumber };
            var result = await useCase.GetByAccountNumber(request);
            return Ok(result);
        }


        [HttpGet]
        [Route("Get-Extract")]
        [ProducesResponseType(typeof(ResponseExtractJson), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExtract(
            [FromServices] IGetAccountUseCase useCase,
            [FromQuery] string accountNumber)
        {
            var request = new RequestAccountJson { AccountNumber = accountNumber };
            var result = await useCase.GetExtract(request);
            return Ok(result);
        }
    }
}
