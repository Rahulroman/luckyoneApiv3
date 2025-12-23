using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace luckyoneApiv3.Entity
{
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(100)]
        [Required]
        public string Username { get; set; }    = string.Empty;
        public string? Email { get; set; } = null;

        public string? FirstName { get; set; } = null;

        public string? LastName { get; set; } = null;

        public string? AvatarUrl { get; set; } = null;
        public int? Points { get; set; }
        public string? Role { get; set; } = "User";

        public bool IsActive { get; set; } = true;


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

       
        public string? RefreshToken { get; set; } = null;

        public DateTime? RefreshTokenExpiry { get; set; } = new DateTime();

       
        public bool? EmailVerified { get; set; } = false;

        public string? VerificationToken { get; set; } = null;
    }
}
