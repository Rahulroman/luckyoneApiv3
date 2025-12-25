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
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> register([FromForm] RegisterRequestDto request) 
        {
           var response = await _authService.Register(request);
            return Ok(response);
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> login([FromBody] LoginRequestDto requestDto) 
        {
            var response = await _authService.loginDto(requestDto);
            return Ok(response);
        }


     

    }
}
