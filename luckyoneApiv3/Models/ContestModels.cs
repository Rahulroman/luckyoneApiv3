using System.ComponentModel.DataAnnotations;

namespace luckyoneApiv3.Models
{
    public class ContestModels
    {
        public class ContestDTO
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string? BannerImage { get; set; }
            public decimal EntryPoints { get; set; }
            public int MaxParticipants { get; set; }
            public int CurrentParticipants { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string Status { get; set; }
            public string? WinnerId { get; set; }
            public string? WinnerUsername { get; set; }
            public string CreatedById { get; set; }
            public string CreatedByUsername { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }
            public bool HasJoined { get; set; }
        }


        public class CreateContestRequest
        {
            [Required]
            [MaxLength(200)]
            public string Title { get; set; }

            [Required]
            [MaxLength(2000)]
            public string Description { get; set; }

            public string? BannerImage { get; set; }

            [Required]
            [Range(1, 1000)]
            public int EntryPoints { get; set; }

            [Required]
            [Range(2, 1000)]
            public int MaxParticipants { get; set; }

            [Required]
            public DateTime StartDate { get; set; }

            [Required]
            public DateTime EndDate { get; set; }
        }

    }
}
