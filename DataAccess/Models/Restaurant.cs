
namespace DataAccess.Models
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = "Cafe";
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public List<RestaurantBreakfast> RestaurantBreakfasts { get; set; } = new();
    }
}
