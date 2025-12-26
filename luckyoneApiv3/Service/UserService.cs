using luckyoneApiv3.Data;
using luckyoneApiv3.Entity;
using luckyoneApiv3.Models;
using luckyoneApiv3.Service.IService;
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
                                  Points =  (decimal)u.Points, // Fix: Handle nullable int conversion
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








    }
}
