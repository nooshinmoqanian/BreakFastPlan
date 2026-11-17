
namespace DataAccess.Models
{
    public class BreakfastIngredient
    {
        public int Id { get; set; }
        public int BreakfastId { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Amount { get; set; }
        public string Unit { get; set; } = string.Empty;
        public int Calories { get; set; }
        public double Price { get; set; }
        public Breakfast? Breakfast { get; set; }
    }
}
