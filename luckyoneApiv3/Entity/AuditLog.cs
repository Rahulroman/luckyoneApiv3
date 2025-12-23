using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace luckyoneApiv3.Entity
{
    public class AuditLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string? UserId { get; set; }

        // 🔹 Action name (Create, Update, Delete, Login, etc.)
        public string Action { get; set; } = string.Empty;

        // 🔹 Entity name (User, Contest, Payment, etc.)
        public string EntityType { get; set; } = string.Empty;

        // 🔹 Affected entity ID
        public string? EntityId { get; set; }

        public string? OldValues { get; set; }

        public string? NewValues { get; set; }

        // 🔹 Client info
        [MaxLength(45)]
        public string? IPAddress { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
