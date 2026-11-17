using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.DTO
{
    public class BreakfastListDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Range(0, double.MaxValue)]
        public double Price { get; set; }
        public string Recipe { get; set; } = string.Empty;
        public List<TagDto> TagNames { get; set; } = new();
        public List<IngredientDto> Ingredients { get; set; } = new();
        public List<HealthInfoDto> HealthInfos { get; set; } = new();
    }

    public class BreakfastDetailDto : BreakfastListDto
    {
        public int Id { get; set; }
        public int TotalCalories { get; set; }
        public double IngredientsPrice { get; set; }
        public List<BreakfastRestaurantDto> Restaurants { get; set; } = new();
    }

    public class IngredientDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Range(0, double.MaxValue)]
        public double Amount { get; set; }
        public string Unit { get; set; } = string.Empty;
        [Range(0, int.MaxValue)]
        public int Calories { get; set; }
        [Range(0, double.MaxValue)]
        public double Price { get; set; }
    }

    public class HealthInfoDto
    {
        [Required]
        public string Condition { get; set; } = string.Empty;
        public bool IsSuitable { get; set; }
        public string Note { get; set; } = string.Empty;
    }

    public class BreakfastRestaurantDto
    {
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public double Price { get; set; }
    }
}
