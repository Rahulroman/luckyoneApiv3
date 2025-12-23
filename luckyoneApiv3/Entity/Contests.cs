using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace luckyoneApiv3.Entity
{
    public class Contests
    {
        [Key]
        [MaxLength(36)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

      
        public string? Description { get; set; } = string.Empty;

        public string? BannerImage { get; set; }

        public int EntryPoints { get; set; }

        public int MaxParticipants { get; set; }

        public int CurrentParticipants { get; set; } = 0;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        // 🔹 Status: upcoming / active / completed / cancelled
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "upcoming";

        public string? WinnerId { get; set; }

        public string CreatedBy { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public int MinimumParticipants { get; set; } = 2;

    }
}
