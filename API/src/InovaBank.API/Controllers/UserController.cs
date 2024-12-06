using InovaBank.Application.UseCases.User.Register;
using InovaBank.Communication.Requests;
using InovaBank.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace InovaBank.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
        public async Task<IActionResult> Register(
            [FromServices] IRegisterUserUseCase useCase,
            [FromBody] RequestRegisterUserJson request)
        {
            var result = await useCase.Execute(request); 
            return Created("", result);
        }
    }
}
