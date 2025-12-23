using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace luckyoneApiv3.Entity
{
    public class Payments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 
        
        public string UserId { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        public int PointsAwarded { get; set; }

        public string PaymentMethod { get; set; } = string.Empty;


        public string? TransactionId { get; set; }

        // 🔹 pending / completed / failed / refunded
        public string Status { get; set; } = "pending";

        public DateTime? PaymentDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // 🔹 Currency (ISO-4217)
        [Required]
        [MaxLength(3)]
        public string Currency { get; set; } = "USD";

    }
}
