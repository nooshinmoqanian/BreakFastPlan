
namespace DataAccess.Models
{
    public class BreakfastHealthInfo
    {
        public int Id { get; set; }
        public int BreakfastId { get; set; }
        public string Condition { get; set; } = string.Empty;
        public bool IsSuitable { get; set; }
        public string Note { get; set; } = string.Empty;
        public Breakfast? Breakfast { get; set; }
    }
}
