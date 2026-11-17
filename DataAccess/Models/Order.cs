
namespace DataAccess.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string OrderType { get; set; } = "Ingredients";
        public int BreakfastId { get; set; }
        public string BreakfastName { get; set; } = string.Empty;
        public int? RestaurantId { get; set; }
        public string? RestaurantName { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
