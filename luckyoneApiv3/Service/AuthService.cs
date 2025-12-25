using luckyoneApiv3.Data;
using luckyoneApiv3.Entity;
using luckyoneApiv3.Helper;
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
        private readonly Jwt_Helper _jwtHelper;

        public AuthService(ApplicationDbContext context, Jwt_Helper jwt_Helper)
        {
            _context = context;
            _jwtHelper = jwt_Helper;
        }

        public async Task<ApiResponseResgisterDto> Register(RegisterRequestDto request)
        {

            var result = await (
                             from U in _context.User
                             where U.Username == request.Username && U.PasswordHash == request.PasswordHash
                             select U
                         ).FirstOrDefaultAsync();

            if (result != null)
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

                var fullPath = Path.Combine(folderPath, fileName.ToString()+".jpg");

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

        public async Task<ApiResponseLoginDto> loginDto(LoginRequestDto requestDto)
        {
            var user = await (from U in _context.User
                              where U.Username == requestDto.Username && U.PasswordHash == requestDto.Password
                              select U
                                ).FirstOrDefaultAsync();

            if (user == null)
            {
                return new ApiResponseLoginDto
                {
                    IsSuccess = false,
                    Message = "Invalid Username or Password",
                    Token = "",
                    User = null
                };
            }

            string token = _jwtHelper.GenrateJwtToken(user.Id, user.Role);

            return new ApiResponseLoginDto
            {
                IsSuccess = true,
                Message = "Login Successful",
                Token = token,
                User = user
            };

        }
    }
}
