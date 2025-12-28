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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
    }
}
