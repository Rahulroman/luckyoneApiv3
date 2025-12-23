using Microsoft.EntityFrameworkCore.Storage;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace luckyoneApiv3.Entity
{
    public class PointsTransactions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? UserId { get; set; } = string.Empty;

        public int? Amount { get; set; }

        // 🔹 credit / debit

        public string? Type { get; set; } = string.Empty;

        public string? ContestId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int? BalanceAfter { get; set; }

        public string? TransactionMethod { get; set; }

        // 🔹 pending / completed / failed / refunded
        public string? Status { get; set; } = "completed";
    }
}
