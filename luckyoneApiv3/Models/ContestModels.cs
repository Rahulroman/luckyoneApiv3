namespace luckyoneApiv3.Models
{
    public class ContestModels
    {

        public class CreateContest
        {
            public int? UserID { get; set; }
            public string? Title { get; set; }
            public string? Description { get; set; }
            public int? EntryPoints { get; set; }
            public int MaxParticipants { get; set; }
            public int? MinimumParticipants { get; set; } 
            public int CurrentParticipants { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
            public string Status { get; set; } = "upcoming";
            
        }
    }
}
