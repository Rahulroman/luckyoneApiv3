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
        Task<bool> DeleteContest (int contestID);

        Task<bool> JoinContest(int contestID , int userID);

        Task<List<ContestParticipantDTO>> GetContestParticipants(int contestID);
        Task<ContestDTO> DeclareWinner(int contestID , int winnerID);

        Task<List<ContestDTO>> GetMyContests(int userID);

        Task<PaginatedResponse<ContestDTO>> GetAdminContests(int page, int limit , int userID);
    }
}
