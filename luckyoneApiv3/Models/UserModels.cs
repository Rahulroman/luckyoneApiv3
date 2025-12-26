using System.Runtime.CompilerServices;

namespace luckyoneApiv3.Models
{
    public class UserModels
    {
        public class UserDto
        {
            public string Id { get; set; }
            public string Username { get; set; }
            public string? Email { get; set; }
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public IFormFile? Avatar { get; set; }
            public decimal Points { get; set; }
            public string Role { get; set; }
            public bool IsActive { get; set; }
            public string? Bio { get; set; }
            public string? Phone { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }
        }














    }
}
