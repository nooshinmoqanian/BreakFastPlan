
namespace DataAccess.Models
{
    public class RestaurantBreakfast
    {
        public int RestaurantId { get; set; }
        public int BreakfastId { get; set; }
        public double Price { get; set; }
        public Restaurant? Restaurant { get; set; }
        public Breakfast? Breakfast { get; set; }
    }
}
