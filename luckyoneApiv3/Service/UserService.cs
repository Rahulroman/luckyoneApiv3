using luckyoneApiv3.Data;
using luckyoneApiv3.Entity;
using luckyoneApiv3.Helper;
using luckyoneApiv3.Models;
using luckyoneApiv3.Service.IService;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static luckyoneApiv3.Models.UserModels;

namespace luckyoneApiv3.Service
{
    public class UserService : IUserService
    {

        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserDto> GetUserById(int id)
        {
            var User = await (from u in _context.User
                              where u.Id == id
                              select new UserDto
                              {
                                  Id = u.Id.ToString(),
                                  Username = u.Username,
                                  Email = u.Email,
                                  FirstName = u.FirstName,
                                  LastName = u.LastName,
                                  Points = u.Points ?? 0, // Fix: Provide a default value for nullable Points
                                  Role = u.Role ?? "User",
                                  IsActive = u.IsActive,
                                  Bio = "",
                                  Phone = "",
                                  CreatedAt = u.CreatedAt,
                                  UpdatedAt = u.UpdatedAt
                              }
                             ).FirstOrDefaultAsync();

            return User;
        }

        public async Task<UserDto> UpdateUserProfile(UpdateProfileRequest request,int userId)
        {
            var User = await (from u in _context.User
                              where u.Id == userId
                              select u
                              ).FirstOrDefaultAsync();

            if (User == null)
            {
                throw new Exception("User not found.");
            }

            if(request.Username == User.Username)
            {
                throw new Exception("Username is already taken.");
            }


            User.Id = User.Id;
            User.Username = request.Username ?? User.Username;
            User.Email = request.Email ?? User.Email;
            User.FirstName = request.FirstName ?? User.FirstName;
            User.LastName = request.LastName ?? User.LastName;
            User.UpdatedAt = DateTime.UtcNow;
           

            await _context.SaveChangesAsync();

            return new UserDto
            {
                Id = User.Id.ToString(),
                Username = User.Username,
                Email = User.Email,
                FirstName = User.FirstName,
                LastName = User.LastName,
                Points = (decimal)User.Points,
                Role = User.Role ?? "User",
                IsActive = User.IsActive,
                Bio = "",
                Phone = "",
                CreatedAt = User.CreatedAt,
                UpdatedAt = User.UpdatedAt
            };

        }

        public async Task<PaginatedResponse<UserDto>> GetAllUsers(int page, int limit, string? search)
        {
            var users = await (from u in _context.User
                              where string.IsNullOrEmpty(search) || u.Username.Contains(search)
                               select new UserDto
                               {
                                   Id = u.Id.ToString(),
                                   Username = u.Username,
                                   Email = u.Email,
                                   FirstName = u.FirstName,
                                   LastName = u.LastName,
                                   Points = (decimal)u.Points,
                                   Role = u.Role ?? "User",
                                   IsActive = u.IsActive,
                                   Bio = "",
                                   Phone = "",
                                   CreatedAt = u.CreatedAt,
                                   UpdatedAt = u.UpdatedAt
                               }
                ).ToListAsync();

            var totalUsers = users.Count;
            var paginatedUsers = users.Skip((page - 1) * limit).Take(limit).ToList();

            return new PaginatedResponse<UserDto>
            {
                data = paginatedUsers,
                page = page,
                limit = limit,
                total = totalUsers
            };



        }




    }
}
