using luckyoneApiv3.Entity;
using luckyoneApiv3.Helper;
using luckyoneApiv3.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static luckyoneApiv3.Models.UserModels;

namespace luckyoneApiv3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly Jwt_Helper _jwt_Helper;

        public UserController(IUserService userService, Jwt_Helper jwt_Helper )
        {
            _userService = userService;
            _jwt_Helper = jwt_Helper;
        }

        [HttpGet]
        [Route("profile")]
        public async Task<IActionResult> GetProfile()
        { 
            var UserId = _jwt_Helper.GetUserIdToken();

            if (UserId <= 0)
            {
                return Unauthorized(new { success = false,  message = "Invalid User ID in token." });
            }

            try
            {
                var response = await _userService.GetUserById(UserId);
                return Ok(new ApiResponse<UserDto>
                {
                    Success = true,
                    Message = "User profile retrieved successfully.",
                    Data = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = ex.Message,
                });
               
            }





        }











    }
}
