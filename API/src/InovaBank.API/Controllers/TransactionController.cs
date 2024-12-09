using InovaBank.Application.UseCases.Transaction.Register;
using InovaBank.Communication.Requests.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InovaBank.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        [HttpPost]
        [Route("Deposit")]
        public async Task<IActionResult> Deposit(
            [FromBody] RequestTransactionJson request,
            [FromServices] ITransactionUseCase useCase
            )
        {
            await useCase.Deposit( request );
            return Ok();
        }

        [HttpPost]
        [Authorize]
        [Route("Withdrawal")]
        public async Task<IActionResult> Withdrawal(
            [FromBody] RequestTransactionJson request,
            [FromServices] ITransactionUseCase useCase
            )
        {
            await useCase.Withdrawal(request);
            return Ok();
        }

        [HttpPost]
        [Authorize]
        [Route("Transfer")]
        public async Task<IActionResult> Transfer(
           [FromBody] RequestTransactionJson request,
           [FromServices] ITransactionUseCase useCase
           )
        {
            await useCase.Transfer(request);
            return Ok();
        }
    }
}
