using InovaBank.Application.UseCases.User.Login;
using InovaBank.Application.UseCases.User.Register;
using InovaBank.Communication.Requests.User;
using InovaBank.Communication.Responses.User;
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

        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login(
            [FromServices] IDoLoginUseCase useCase,
            [FromBody] RequestLoginJson request)
        {
            var result = await useCase.Execute(request);
            return Ok(result);
        }
    }
}
