
namespace DataAccess.Models
{
    public class Breakfast
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Recipe { get; set; } = string.Empty;
        public List<BreakfastTag> BreakfastTags { get; set; } = new();
        public List<BreakfastIngredient> Ingredients { get; set; } = new();
        public List<BreakfastHealthInfo> HealthInfos { get; set; } = new();
        public List<RestaurantBreakfast> RestaurantBreakfasts { get; set; } = new();
    }
}
