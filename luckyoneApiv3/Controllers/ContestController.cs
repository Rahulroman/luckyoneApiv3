using luckyoneApiv3.Helper;
using luckyoneApiv3.Service.IService;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
using static luckyoneApiv3.Models.ContestModels;

namespace luckyoneApiv3.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    
    public class ContestController : ControllerBase
    {
        private readonly IContestService _contestService;
        private readonly Jwt_Helper _jwt_Helper;
        public ContestController(IContestService contestService, Jwt_Helper jwt_Helper)
        {
            _contestService = contestService;
            _jwt_Helper = jwt_Helper;
        }


        [HttpPost("CreateContest")]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<ApiResponse<ContestDTO>>> CreateContest([FromBody] CreateContestRequest request)
        {
            var UserId = _jwt_Helper.GetUserIdToken();

            if (UserId <= 0)
            {
                return Unauthorized(new ApiResponse<ContestDTO>
                {
                    success = false,
                    message = "You are Not Allowed to Create Contest.",
                    data = null
                });
            }

            try {
                
                var result = await _contestService.CreateContest(request , UserId);

                return Ok(new ApiResponse<ContestDTO>
                {
                    success = true,
                    message = "Contest Created Successfully.",
                    data = result
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<ContestDTO>
                {
                    success = false,
                    message = ex.Message,
                    data = null
                });
            }




        }








    }
}
