using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace luckyoneApiv3.Entity
{
    public class Winners
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 

        public string ContestId { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public int PrizePoints { get; set; }

        public string DeclaredBy { get; set; } = string.Empty;

        public DateTime DeclaredAt { get; set; } = DateTime.UtcNow;

        // 🔹 Rank / position
        public int Position { get; set; }
    }
}
