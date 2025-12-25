using luckyoneApiv3.Entity;
using luckyoneApiv3.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace luckyoneApiv3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet]
        [Route("GetAllUserList")]
        public async Task<ActionResult<List<Users>>> GetAllUserList()
        {
            var response = await _userService.GetAllUserList();

            return Ok(new { UserList = response });

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserByID(int id)
        {
          var response = await _userService.GetUserByID(id);
            return Ok(response);
        }








    }
}
