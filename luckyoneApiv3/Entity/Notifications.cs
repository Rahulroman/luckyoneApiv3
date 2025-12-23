using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace luckyoneApiv3.Entity
{
    public class Notifications
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 

        public string UserId { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        // 🔹 contest / points / winner / system / payment
        public string Type { get; set; } = string.Empty;

        // 🔹 Read flag
        public bool IsRead { get; set; } = false;

        // 🔹 Created time
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // 🔹 Optional reference (ContestId, PaymentId, etc.)
        [MaxLength(36)]
        public string? RelatedId { get; set; }
    }
}
