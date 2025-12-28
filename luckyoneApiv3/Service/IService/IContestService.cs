using luckyoneApiv3.Entity;
using luckyoneApiv3.Helper;
using static luckyoneApiv3.Models.ContestModels;

namespace luckyoneApiv3.Service.IService
{
    public interface IContestService
    {
        Task<ContestDTO> CreateContest(CreateContestRequest request ,int UserId);
        Task<ContestDTO> GetContestById(string id , string contestID);
        Task<PaginatedResponse<ContestDTO>> GetContest(int page, int limit, string status, int userID);
        Task<ContestDTO> UpdateContest (UpdateContestRequest contestDTO , int contestID);

    }
}
