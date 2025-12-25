using luckyoneApiv3.Entity;
using Microsoft.AspNetCore.Http;

namespace luckyoneApiv3.Models
{
    public class AuthModels
    {
        public class RegisterRequestDto
        {
            public string Username { get; set; } = string.Empty;
            public string? Email { get; set; } = null;

            public string? FirstName { get; set; } = null;

            public string? LastName { get; set; } = null;

            public IFormFile? AvatarImage { get; set; } = null;

            public string PasswordHash { get; set; } = string.Empty;
        }


        public class ApiResponseResgisterDto 
        { 
             public Boolean IsSuccess {  get; set; }
             public string Message { get; set; }
        }

        public class ApiResponseLoginDto
        {
            public Boolean IsSuccess { get; set; }
            public string Message { get; set; }
            public string Token { get; set; }

            public Users? User { get; set; }
        }

        public class LoginRequestDto
        {
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }



    }
}
