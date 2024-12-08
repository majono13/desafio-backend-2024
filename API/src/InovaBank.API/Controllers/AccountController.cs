using InovaBank.Application.UseCases.User.Register;
using InovaBank.Communication.Requests;
using InovaBank.Communication.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InovaBank.API.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() {
            return Ok("Token Passou");
        }
    }
}
