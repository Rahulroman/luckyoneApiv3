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
        private readonly IContestService contestService;
        public ContestController(IContestService contestService)
        {
            this.contestService = contestService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("CreateContest")]
        public async Task<IActionResult> CreateContest([FromBody] CreateContest joinContest)
        {
            var result = await contestService.CreateContest(joinContest);
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
