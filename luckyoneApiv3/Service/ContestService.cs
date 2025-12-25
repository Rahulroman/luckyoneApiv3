using luckyoneApiv3.Data;
using luckyoneApiv3.Entity;
using luckyoneApiv3.Service.IService;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static luckyoneApiv3.Models.ContestModels;

namespace luckyoneApiv3.Service
{
    public class ContestService : IContestService
    {

        private readonly ApplicationDbContext _context;
        public ContestService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<Contests> JoinContest(CreateContest joinContest, string UserID)
        { 
            var contest = await (from C in _context.Contests
                                 where C.Title == joinContest.Title
                                 select C
                                 ).FirstOrDefaultAsync();

            if (contest != null)
            {
                return null;
            }

            try
            {
                var Contest = new Contests
                {

                    Title = joinContest.Title,
                    Description = joinContest.Description,
                    // BannerImage = joinContest.BannerImage,
                    EntryPoints = joinContest.EntryPoints ?? 0,
                    MaxParticipants = joinContest.MaxParticipants,
                    StartDate = DateTime.Parse(joinContest.StartDate),
                    EndDate = DateTime.Parse(joinContest.EndDate),
                    Status = "upcoming",
                    CreatedBy = UserID,
                    CreatedAt = DateTime.UtcNow,
                    MinimumParticipants = joinContest.MinimumParticipants ?? 0
                };

                await _context.Contests.AddAsync(Contest);
                await _context.SaveChangesAsync();

                return Contest;
            }
            catch (Exception ex)
            {
                return null;
            }

       


        }

        public async Task<List<Contests>> GetAllContests()
        {
            var contests = await _context.Contests.ToListAsync();
            return contests;
        }






    }
}
