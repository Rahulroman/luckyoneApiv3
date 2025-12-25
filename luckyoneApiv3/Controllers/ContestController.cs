using luckyoneApiv3.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static luckyoneApiv3.Models.ContestModels;

namespace luckyoneApiv3.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    
    public class ContestController : ControllerBase
    {
        private readonly IContestService contestService;
        public ContestController(IContestService contestService)
        {
            this.contestService = contestService;
        }

        [HttpPost]
        [Route("CreateContest")]
        public async Task<IActionResult> JoinContest([FromBody] CreateContest joinContest ,string UserId)
        {
            var result = await contestService.JoinContest(joinContest , UserId);
            return Ok(result);
        }


        [HttpGet]
        [Route("GetAllContests")]
        public async Task<IActionResult> GetAllContests()
        {
            var result = await contestService.GetAllContests();
            return Ok(result);
        }

    }
}
