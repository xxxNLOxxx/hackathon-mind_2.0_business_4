namespace Hackaton.Models
{
    public class UserRating
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public int? TotalScore { get; set; }
        public int Rank { get; set; } 
    }
}
