using luckyoneApiv3.Helper;
using static luckyoneApiv3.Models.PointsModel;

namespace luckyoneApiv3.Service.IService
{
    public interface IPointsService
    {
        Task<PointsBalanceDTO> GetBalance(int userId);
        Task<PaginatedResponse<PointsTransactionDTO>> GetTransactions(int page , int limit , int userId);
        Task<PointsTransactionDTO> AddPoints(int userID, AddPointsRequest request);
        Task<bool> DeductPoints(int userID, int points, int contestID);
    }
}
