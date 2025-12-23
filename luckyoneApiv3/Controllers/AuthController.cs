using luckyoneApiv3.Entity;
using luckyoneApiv3.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using static luckyoneApiv3.Models.AuthModels;

namespace luckyoneApiv3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;
        public AuthController(IAuthService authService) 
        {
            _authService = authService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]

        public async Task<IActionResult> register([FromBody] RegisterRequestDto request) 
        {
           var response = await _authService.Register(request);

            return Ok(response);
        }


        [HttpGet]
        [Route("GetAllUserList")]
        public async Task<ActionResult<List<Users>>> GetAllUserList()
        { 
          var response = await _authService.GetAllUserList();

            return Ok(response );

        }

    }
}
