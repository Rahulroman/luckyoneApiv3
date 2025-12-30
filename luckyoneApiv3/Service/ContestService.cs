using luckyoneApiv3.Data;
using luckyoneApiv3.Entity;
using luckyoneApiv3.Helper;
using luckyoneApiv3.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static luckyoneApiv3.Models.ContestModels;
using static luckyoneApiv3.Models.PointsModel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace luckyoneApiv3.Service
{
    public class ContestService : IContestService
    {

        private readonly ApplicationDbContext _context;
        private readonly IPointsService _pointService;
        private readonly Jwt_Helper _jwt_Helper;
        // Add IHttpContextAccessor
        public ContestService(ApplicationDbContext context, Jwt_Helper jwt_Helper, IPointsService pointService)
        {
            _context = context;
            _jwt_Helper = jwt_Helper;
            _pointService = pointService;
        }

        public async Task<ContestDTO> CreateContest(CreateContestRequest request , int UserId)
        {
            if (request.StartDate <= DateTime.Now)
            {
                throw new Exception("Start date must be in the future");
            }
            if (request.EndDate <= request.StartDate)
            {
                throw new Exception("End date must be after start date");
            }

            var contest = new Contests {
                Title = request.Title,
                Description = request.Description,
                BannerImage = request.BannerImage ?? "",
                EntryPoints = request.EntryPoints,
                MaxParticipants = request.MaxParticipants,
                CurrentParticipants = 0,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Status = "Upcoming",
                CreatedBy = UserId.ToString(),
                CreatedAt = DateTime.UtcNow,
            };

            await _context.Contests.AddAsync(contest);
            await _context.SaveChangesAsync();

            return new ContestDTO
            {
                Id = contest.Id.ToString(),
                Title = contest.Title,
                Description = contest.Description,
                BannerImage = contest.BannerImage,
                EntryPoints = contest.EntryPoints,
                MaxParticipants = contest.MaxParticipants,
                CurrentParticipants = contest.CurrentParticipants,
                StartDate = contest.StartDate,
                EndDate = contest.EndDate,
                Status = contest.Status,
                WinnerId = contest.WinnerId,
                CreatedById = contest.CreatedBy,
                CreatedAt = contest.CreatedAt,
                UpdatedAt = contest.UpdatedAt,
                HasJoined = false
            };

        }

        public async Task<ContestDTO> GetContestById(string id, string contestID)
        {
            var result = await (from c in _context.Contests
                                where c.Id == int.Parse(contestID)  && c.CreatedBy == id
                                select new ContestDTO
                                {
                                    Id = c.Id.ToString(),
                                    Title = c.Title,
                                    Description = c.Description,
                                    BannerImage = c.BannerImage,
                                    EntryPoints = c.EntryPoints,
                                    MaxParticipants = c.MaxParticipants,
                                    CurrentParticipants = c.CurrentParticipants,
                                    StartDate = c.StartDate,
                                    EndDate = c.EndDate,
                                    Status = c.Status ?? string.Empty,
                                    WinnerId = c.WinnerId ?? string.Empty,
                                     CreatedAt = c.CreatedAt,
                                    UpdatedAt = c.UpdatedAt,
                                  //  HasJoined = false
                                }).FirstOrDefaultAsync();


            return result;
        }

        public async Task<PaginatedResponse<ContestDTO>> GetContest(int page, int limit, string status, int userID)
        {
            var query = await  (from c in _context.Contests
                            where c.CreatedBy == userID.ToString() && c.Status == status
                            orderby c.CreatedAt descending
                            select new ContestDTO {
                                Id = c.Id.ToString(),
                                Title = c.Title,
                                Description = c.Description,
                                BannerImage = c.BannerImage,
                                EntryPoints = c.EntryPoints,
                                MaxParticipants = c.MaxParticipants,
                                CurrentParticipants = c.CurrentParticipants,
                                StartDate = c.StartDate,
                                EndDate = c.EndDate,
                                Status = c.Status ?? string.Empty,
                                WinnerId = c.WinnerId ?? string.Empty,
                                CreatedAt = c.CreatedAt,
                                UpdatedAt = c.UpdatedAt,
                            }
                            ).ToListAsync();


            var contests = query.Skip((page - 1) * limit).Take(limit).ToList();


            var total = contests.Count;
            var totalPages = contests.Count;



            return new PaginatedResponse<ContestDTO> { 
            
                total = total,
                totalPages = (int)Math.Ceiling(total / (double)limit),
                page = page,
                limit = limit,
                data = contests

            };



        }

        public async Task<ContestDTO> UpdateContest(UpdateContestRequest request, int contestID)
        {
            var contest = await (from c in _context.Contests
                                 where c.Id == contestID
                                 select c
                                 ).FirstOrDefaultAsync();

            if (contest == null) {
                throw new Exception("contest not found");
            }

            // Update fields if provided
            if (!string.IsNullOrEmpty(request.Title))
                contest.Title = request.Title;

            if (!string.IsNullOrEmpty(request.Description))
                contest.Description = request.Description;

            if (request.BannerImage != null)
                contest.BannerImage = request.BannerImage;

            if (request.EntryPoints.HasValue)
                contest.EntryPoints = (int)request.EntryPoints.Value;

            if (request.MaxParticipants.HasValue)
                contest.MaxParticipants = request.MaxParticipants.Value;

            if (request.StartDate.HasValue)
                contest.StartDate = request.StartDate.Value.ToUniversalTime();

            if (request.EndDate.HasValue)
                contest.EndDate = request.EndDate.Value.ToUniversalTime();

            if (!string.IsNullOrEmpty(request.Status))
                contest.Status = request.Status;

            contest.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new ContestDTO {
                Id = contest.Id.ToString(),
                Title = contest.Title,
                Description = contest.Description,
                BannerImage = contest.BannerImage,
                EntryPoints = contest.EntryPoints,
                MaxParticipants = contest.MaxParticipants,
                CurrentParticipants = contest.CurrentParticipants,
                StartDate = contest.StartDate,
                EndDate = contest.EndDate,
                Status = contest.Status ?? string.Empty,
                WinnerId = contest.WinnerId ?? string.Empty,
                CreatedAt = contest.CreatedAt,
                UpdatedAt = contest.UpdatedAt,
            };


        }

        public async Task<bool> DeleteContest(int contestID)
        {
            var contest = await (from c in _context.Contests
                                 where  c.Id == contestID
                                 select c).FirstOrDefaultAsync();

            if (contest == null)
            {
                throw new Exception("Contest not found");
            }
            if (contest.Status != "upcoming")
            {
                throw new Exception("Can only delete upcoming contests with no participants");
            }


             _context.Contests.Remove(contest);
            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<bool> JoinContest(int contestID, int userID)
        {
            var contest = await (from c in _context.Contests
                                 join p in _context.ContestParticipants on c.Id equals p.ContestId
                                 where c.Id == userID
                                 select c).FirstOrDefaultAsync();

            if (contest == null) 
            {
                throw new Exception("contest Not Found");
            }

            // Check contest status
            if (contest.Status != "active")
            {
                throw new Exception("Contest is not active");
            }

            // Check if user has enough points
            var user = await _context.User.FindAsync(userID);
            if (user == null || user.Points < contest.EntryPoints)
            {
                throw new Exception("Insufficient points");
            }


            // Deduct points
            await _pointService.DeductPoints(userID, contest.EntryPoints, contestID);

            var participant = new ContestParticipants
            {
                ContestId = contestID,
                UserId = userID,
                PointsSpent = contest.EntryPoints,
                JoinedAt = DateTime.UtcNow
            };

             _context.ContestParticipants.Add(participant);

            contest.CurrentParticipants++;
            contest.UpdatedAt = DateTime.UtcNow;

            await   _context.SaveChangesAsync();

            return true;


        }

        public async Task<List<ContestParticipantDTO>> GetContestParticipants(int contestID)
        {
            var contest = await (from p in _context.ContestParticipants
                                 join u in _context.User on p.UserId equals u.Id
                                 where p.ContestId == contestID 
                                 orderby p.JoinedAt
                                 select  new ContestParticipantDTO {
                                     Id = p.Id.ToString(),
                                     ContestId = p.ContestId.ToString(),
                                     UserId = p.UserId.ToString(),
                                     Username = u.Username,
                                     Avatar = u.AvatarUrl,
                                     PointsSpent = p.PointsSpent,
                                     JoinedAt = p.JoinedAt
                                 }).ToListAsync();

            return contest;

        }

        public async Task<ContestDTO> DeclareWinner(int contestID, int winnerID)
        {
            var contest = await (from c in _context.Contests
                                 where c.Id == contestID
                                 select c).FirstOrDefaultAsync();

            if (contest == null)
            {
                throw new Exception("Contest not found");
            }


            if(contest.Status != "completed")
            {
                throw new Exception("Can only declare winner for completed contests");
            }

            var winnerParticipant = await (from p in _context.ContestParticipants
                                           where p.ContestId == contestID && p.UserId == winnerID
                                           select p).FirstOrDefaultAsync();

            if (winnerParticipant == null)
            {
                throw new Exception("Winner must be a participant of the contest");
            }

            var prizePoints = (int)(contest.EntryPoints * contest.CurrentParticipants * 0.6); // 60% of total points

            contest.WinnerId = winnerID.ToString();
            contest.Status = "completed";
            contest.UpdatedAt = DateTime.UtcNow;

            await _pointService.AddPoints(winnerID, new AddPointsRequest { Amount = prizePoints, PaymentMethod = "contest_prize", TransactionId = $"CONTEST_{contestID}_WIN" });

            var OtherParticipants = await (from p in _context.ContestParticipants
                                           where p.UserId != winnerID
                                           orderby p.JoinedAt
                                           select p).ToListAsync();

            await _context.SaveChangesAsync();

            return new ContestDTO
            {
                Id = contest.Id.ToString(),
                Title = contest.Title,
                Description = contest.Description,
                BannerImage = contest.BannerImage,
                EntryPoints = contest.EntryPoints,
                MaxParticipants = contest.MaxParticipants,
                CurrentParticipants = contest.CurrentParticipants,
                StartDate = contest.StartDate,
                EndDate = contest.EndDate,
                Status = contest.Status ?? string.Empty,
                WinnerId = contest.WinnerId ?? string.Empty,
                CreatedAt = contest.CreatedAt,
                UpdatedAt = contest.UpdatedAt,
            };

        }

        public async Task<List<ContestDTO>> GetMyContests(int userID)
        {
           var contest = await (from c in _context.Contests
                                where c.CreatedBy == userID.ToString()
                                orderby c.CreatedAt descending
                                select new ContestDTO
                                {
                                    Id = c.Id.ToString(),
                                    Title = c.Title,
                                    Description = c.Description,
                                    BannerImage = c.BannerImage,
                                    EntryPoints = c.EntryPoints,
                                    MaxParticipants = c.MaxParticipants,
                                    CurrentParticipants = c.CurrentParticipants,
                                    StartDate = c.StartDate,
                                    EndDate = c.EndDate,
                                    Status = c.Status ?? string.Empty,
                                    WinnerId = c.WinnerId ?? string.Empty,
                                    CreatedAt = c.CreatedAt,
                                    UpdatedAt = c.UpdatedAt,
                                }).ToListAsync();

            return contest;
        }

        public async Task<PaginatedResponse<ContestDTO>> GetAdminContests(int page, int limit, int userID)
        {
            var query = await (from c in _context.Contests
                               where c.CreatedBy == userID.ToString()
                               orderby c.CreatedAt descending
                               select new ContestDTO
                               {
                                   Id = c.Id.ToString(),
                                   Title = c.Title,
                                   Description = c.Description,
                                   BannerImage = c.BannerImage,
                                   EntryPoints = c.EntryPoints,
                                   MaxParticipants = c.MaxParticipants,
                                   CurrentParticipants = c.CurrentParticipants,
                                   StartDate = c.StartDate,
                                   EndDate = c.EndDate,
                                   Status = c.Status ?? string.Empty,
                                   WinnerId = c.WinnerId ?? string.Empty,
                                   CreatedAt = c.CreatedAt,
                                   UpdatedAt = c.UpdatedAt,
                               }).ToListAsync();


            var contests = query.Skip((page - 1) * limit).Take(limit).ToList();

            var total = contests.Count;

            return new PaginatedResponse<ContestDTO>
            {
                total = total,
                totalPages = (int)Math.Ceiling(total / (double)limit),
                page = page,
                limit = limit,
                data = contests

            };
        }




    }
}
