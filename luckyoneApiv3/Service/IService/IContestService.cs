using luckyoneApiv3.Entity;
using static luckyoneApiv3.Models.ContestModels;

namespace luckyoneApiv3.Service.IService
{
    public interface IContestService
    {
        Task<Contests> JoinContest(CreateContest joinContest, string UserID);

        Task<List<Contests>> GetAllContests();
    }
}
