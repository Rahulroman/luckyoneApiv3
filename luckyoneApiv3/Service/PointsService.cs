using luckyoneApiv3.Data;
using luckyoneApiv3.Entity;
using luckyoneApiv3.Helper;
using luckyoneApiv3.Models;
using luckyoneApiv3.Service.IService;
using Microsoft.EntityFrameworkCore;
using static luckyoneApiv3.Models.PointsModel;

namespace luckyoneApiv3.Service
{
    public class PointsService : IPointsService
    {
        private readonly ApplicationDbContext _context;
        public PointsService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<PointsBalanceDTO> GetBalance(int userId)
        {
            var pointBalance = await (from p in _context.User
                                      where p.Id == userId
                                      select p).FirstOrDefaultAsync();

            if (pointBalance != null)
            {
               throw new Exception("User not found");
            }

            return new PointsBalanceDTO
            {
                 Balance = (decimal)pointBalance.Points // Fix for CS1503: Cast nullable int to 
            };


        }

        public async Task<PaginatedResponse<PointsTransactionDTO>> GetTransactions(int page, int limit, int userId)
        {

            var query = from p in _context.PointTransactions
                        join c in _context.Contests
                        on p.ContestId equals c.Id.ToString()
                        where p.UserId == userId.ToString()
                        orderby p.CreatedAt descending
                        select new PointsTransactionDTO
                        {
                            Id = p.Id.ToString(),
                            UserId = p.UserId.ToString(),
                            Amount = p.Amount ?? 0, // Ensure nullable Amount is handled
                            Type = p.Type,
                            Description = c.Description,
                            ContestId = p.ContestId,
                            ContestTitle = c != null ? c.Title : null,
                            PaymentMethod = p.TransactionMethod,
                            CreatedAt = p.CreatedAt
                        };

            var total = query.Count();
            var data = await query.Skip((page - 1) * limit).Take(limit).ToListAsync();

            return new PaginatedResponse<PointsTransactionDTO>
            {
                total = total,
                page = page,
                limit = limit,
                totalPages = (int)Math.Ceiling((double)total / limit),
                data = data
            };

        }


        public async Task<PointsTransactionDTO> AddPoints(int userID, AddPointsRequest request)
        {
            var query = _context.User.FindAsync(userID);

            if (query == null)
            {
                throw new Exception("User not found");
            }

            var transaction = new PointsTransactions
            {
                UserId = userID.ToString(),
                Amount = (int)(request.Amount),
                Type = "credit",
                TransactionMethod = request.PaymentMethod,
                CreatedAt = DateTime.UtcNow
            };

            await _context.PointTransactions.AddAsync(transaction);
            await _context.SaveChangesAsync();

            return 





        }



    }
}
