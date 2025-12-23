using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace luckyoneApiv3.Entity
{
    public class ContestParticipants
    {
        [Key]
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ContestId { get; set; } 

        public int UserId { get; set; } 

        public int PointsSpent { get; set; }


        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        public bool IsWinner { get; set; } = false;

        public int? WonPoints { get; set; }
    }
}
