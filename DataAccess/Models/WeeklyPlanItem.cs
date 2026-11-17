
namespace DataAccess.Models
{
    // One row = one day of one user's week. Day 0 is Saturday, 6 is Friday.
    public class WeeklyPlanItem
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public int Day { get; set; }
        public int BreakfastId { get; set; }
        public Breakfast? Breakfast { get; set; }
    }
}
