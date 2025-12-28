using luckyoneApiv3.Entity;
using luckyoneApiv3.Helper;
using luckyoneApiv3.Service.IService;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using static luckyoneApiv3.Models.ContestModels;

namespace luckyoneApiv3.Controllers
{
   // [Authorize]
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

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ContestDTO>>> GetContestBYId(string id)
        {
            try
            {
                var UserId = _jwt_Helper.GetUserIdToken();
                string contestId = id;

                var response = await _contestService.GetContestById( UserId.ToString() , contestId);

                return Ok(new ApiResponse<ContestDTO> { 
                    success = true,
                    message = "Data Fetch Successfully",
                    data = response
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

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PaginatedResponse<ContestDTO>>>> GetContest([FromQuery]int page, [FromQuery] int limit, [FromQuery] string Status)
        {
            try
            {
                int userID = _jwt_Helper.GetUserIdToken();
                var contests = await _contestService.GetContest(page, limit, Status, userID);
                return Ok(new ApiResponse<PaginatedResponse<ContestDTO>>
                {
                    success = true,
                    data = contests,
                    message = "Contests retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    success = false,
                    message = ex.Message
                });
            }


        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<ContestDTO>>> UpdateContest([FromBody] UpdateContestRequest request , string contestID)
        {

            try
            {
                int userId = _jwt_Helper.GetUserIdToken();

                var response = await _contestService.UpdateContest(request, int.Parse(contestID));

                return Ok(new ApiResponse<ContestDTO>
                {
                    success = true,
                    message = "Update Successful",
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
}
