using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using luckyoneApiv3.Data;
using luckyoneApiv3.Entity;
using luckyoneApiv3.Helper;
using luckyoneApiv3.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static luckyoneApiv3.Models.ContestModels;

namespace luckyoneApiv3.Service
{
    public class ContestService : IContestService
    {

        private readonly ApplicationDbContext _context;
        private readonly Jwt_Helper _jwt_Helper;
        // Add IHttpContextAccessor
        public ContestService(ApplicationDbContext context, Jwt_Helper jwt_Helper)
        {
            _context = context;
            _jwt_Helper = jwt_Helper;
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
    }
}
