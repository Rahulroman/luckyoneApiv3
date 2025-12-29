using luckyoneApiv3.Helper;
using luckyoneApiv3.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static luckyoneApiv3.Models.PointsModel;

namespace luckyoneApiv3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointsController : ControllerBase
    {

        private readonly IPointsService _pointsService;
        private readonly Jwt_Helper _jwtHelper;
        public PointsController(IPointsService pointsService,Jwt_Helper jwt_Helper)
        {
            _pointsService = pointsService;
            _jwtHelper = jwt_Helper;
        }

        [HttpGet("GetBalance")]
        public async Task<ActionResult<ApiResponse<PointsBalanceDTO>>> GetBalance()
        {
            try
            {
                var userID = _jwtHelper.GetUserIdToken();

                var result = await _pointsService.GetBalance(userID);
                return Ok( new ApiResponse<PointsBalanceDTO>
                {
                    success = true,
                    message = "Points balance retrieved successfully.",
                    data = result
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

        [HttpGet("GetTransactions")]
        public async Task<ActionResult<ApiResponse<PaginatedResponse<PointsTransactionDTO>>>> GetTransactions([FromQuery] int page, [FromQuery] int limit)
        {
            try
            {
                var userID = _jwtHelper.GetUserIdToken();

                var result = await _pointsService.GetTransactions(page, limit, userID);
                return Ok(new ApiResponse<PaginatedResponse<PointsTransactionDTO>>
                {
                    success = true,
                    message = "Points transactions retrieved successfully.",
                    data = result
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

        [HttpPost("add")]
        public async Task<ActionResult<ApiResponse<PointsTransactionDTO>> AddPoints(AddPointsRequest request)
        {
            try
            {
                var userID = _jwtHelper.GetUserIdToken();

                var result = await _pointsService.AddPoints(userID, request);

                return Ok(new ApiResponse<PointsTransactionDTO>
                {
                    success = true,
                    message = "Points added successfully.",
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













    }
}
