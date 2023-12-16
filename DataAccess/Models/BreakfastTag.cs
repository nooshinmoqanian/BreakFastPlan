
namespace DataAccess.Models
{
    public class BreakfastTag
    {
        public int BreakfastId { get; set; }
        public Breakfast Breakfast { get; set; }

        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
