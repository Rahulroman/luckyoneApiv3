using luckyoneApiv3.Data;
using luckyoneApiv3.Entity;
using luckyoneApiv3.Service.IService;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using static luckyoneApiv3.Models.AuthModels;

namespace luckyoneApiv3.Service
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponseResgisterDto> Register(RegisterRequestDto request)
        {

            var result = await (
                             from U in _context.User
                             where U.Username == request.Username
                             select U
                         ).FirstOrDefaultAsync();

            if (result == null)
            {
                return new ApiResponseResgisterDto { IsSuccess = false, Message = "UserName Already Exists" };
            }

            string? imageUrl = "";

            if (request.AvatarImage != null)
            {
                var folderPath = Path.Combine("wwwroot", "profiles");

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var fileName = Guid.NewGuid();

                var fullPath = Path.Combine(folderPath, fileName.ToString());

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await request.AvatarImage.CopyToAsync(stream);
                }

                imageUrl = "/profiles/" + fileName; // 👈 store this in DB

            }

            var registeruser = new Users
            {
                Username = request.Username,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                AvatarUrl = imageUrl,
                PasswordHash = request.PasswordHash,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
            };


            await _context.AddAsync(registeruser);
            await _context.SaveChangesAsync();


            return new ApiResponseResgisterDto
            {
                IsSuccess = true,
                Message = "Register Successfully",
            };
        }

        public async Task<List<Users>> GetAllUserList()
        {

            var result = await (from U in _context.User
                                select U
                               ).ToListAsync();

            if (result.Count == 0)
            {
                return null;
            }

            return result;
        }



    }
}
