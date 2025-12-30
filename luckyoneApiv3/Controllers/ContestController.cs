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
        [Authorize(Roles = "Admin")]
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

            try
            {

                var result = await _contestService.CreateContest(request, UserId);

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

        [HttpGet("GetContestBYId/{id}")]
        public async Task<ActionResult<ApiResponse<ContestDTO>>> GetContestBYId(string id)
        {
            try
            {
                var UserId = _jwt_Helper.GetUserIdToken();
                string contestId = id;

                var response = await _contestService.GetContestById(UserId.ToString(), contestId);

                return Ok(new ApiResponse<ContestDTO>
                {
                    success = true,
                    message = "Data Fetch Successfully",
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

        [HttpGet("GetContest")]
        public async Task<ActionResult<ApiResponse<PaginatedResponse<ContestDTO>>>> GetContest([FromQuery] int page, [FromQuery] int limit, [FromQuery] string Status)
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

        [HttpPut("UpdateContest/{id}")]
        // [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<ContestDTO>>> UpdateContest([FromBody] UpdateContestRequest request, string id)
        {

            try
            {
                int userId = _jwt_Helper.GetUserIdToken();

                var response = await _contestService.UpdateContest(request, int.Parse(id));

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


        [HttpDelete("DeleteContest/{id}")]
        // [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse>> DeleteContest(int id)
        {
            try
            {
                var userID = _jwt_Helper.GetUserIdToken();

                var response = await _contestService.DeleteContest(id);

                return Ok(new ApiResponse
                {
                    success = true,
                    message = "Contest deleted successfully"
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


        [HttpPost("JoinContest/{id}")]
        public async Task<ActionResult<ApiResponse>> JoinContest(int id)
        {
            try
            {
                var userID = _jwt_Helper.GetUserIdToken();

                var contest = await _contestService.JoinContest(id, userID);

                return Ok(new ApiResponse
                {
                    success = true,
                    message = $"Join contest with id {id}"

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


        [HttpGet("participants/{contestID}")]
        public async Task<ActionResult<ApiResponse<List<ContestParticipantDTO>>>> GetParticipants(int contestID)
        {
            try
            {
                var userID = _jwt_Helper.GetUserIdToken();

                var result = await _contestService.GetContestParticipants(contestID);

                return Ok(new ApiResponse<List<ContestParticipantDTO>>
                {
                    success = true,
                    message = "Participants retrieved successfully",
                    data = result
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

        [HttpPost("{id}/declare-winner")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<ApiResponse<ContestDTO>>> DeclareWinner(int id, int WinnerId)
        {
            try
            {
                var contest = await _contestService.DeclareWinner(id, WinnerId);
                return Ok(new ApiResponse<ContestDTO>
                {
                    success = true,
                    data = contest,
                    message = "Winner declared successfully"
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

        [HttpGet("MyContests")]
        public async Task<ActionResult<ApiResponse<List<ContestDTO>>>> GetMyContests()
        {
            try
            {
                int userID = _jwt_Helper.GetUserIdToken();
                var contests = await _contestService.GetMyContests(userID);
                return Ok(new ApiResponse<List<ContestDTO>>
                {
                    success = true,
                    message = "My Contests retrieved successfully",
                    data = contests
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


        [HttpGet("GetAdminContests")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<PaginatedResponse<ContestDTO>>>> GetAdminContests([FromQuery] int page, [FromQuery] int limit)
        {
            try
            {
                int userID = _jwt_Helper.GetUserIdToken();
                var contests = await _contestService.GetAdminContests(page, limit, userID);
                return Ok(new ApiResponse<PaginatedResponse<ContestDTO>>
                {
                    success = true,
                    data = contests,
                    message = "Admin Contests retrieved successfully"
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
    
    
    
    }

}
