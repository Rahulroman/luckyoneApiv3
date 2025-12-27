using luckyoneApiv3.Entity;
using luckyoneApiv3.Helper;
using luckyoneApiv3.Service.IService;
using Microsoft.AspNetCore.Authorization;
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
                    success = true,
                    message = "User profile retrieved successfully.",
                    data = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    success = false,
                    message = ex.Message,
                });
            }
        }

        [HttpPost]
        [Route("updateProfile")]
        public async Task<ActionResult<ApiResponse<UserDto>>> UpdateProfile(UpdateProfileRequest request)
        {
            try
            {
                var userId = _jwt_Helper.GetUserIdToken();

                if (userId <= 0)
                {
                    return Unauthorized(new ApiResponse
                    {
                        success = false,
                        message = "Invalid User ID in token.",
                    });
                }

                var result = await _userService.UpdateUserProfile(request , userId);

                return Ok(new ApiResponse<UserDto>
                {
                    success = true,
                    message = "Profile updated successfully.",
                    data= result
                });

            }
            catch (Exception)
            {
                return BadRequest(new ApiResponse
                {
                    success = false,
                    message = "An error occurred while updating the profile.",
                 });
            }
           
        }


        [HttpGet]
        [Route("GetAllUsers")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<PaginatedResponse<UserDto>>>> GetAllUsers(int page, int limit, string? search)
        {
            try
            {
                 var result = await _userService.GetAllUsers(page, limit, search);
                return Ok(new ApiResponse<PaginatedResponse<UserDto>> 
                { 
                    success = true,
                    message = "Users retrieved successfully.",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse {
                    success = false,
                    message = ex.Message,
                });
            }
        }






    }
}
