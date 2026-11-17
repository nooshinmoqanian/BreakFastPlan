using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.DTO
{
    public class RestaurantDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [RegularExpression("Cafe|Restaurant")]
        public string Type { get; set; } = "Cafe";
        [Required]
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public List<MenuItemDto> Menu { get; set; } = new();
    }

    public class MenuItemDto
    {
        public int BreakfastId { get; set; }
        public string BreakfastName { get; set; } = string.Empty;
        [Range(0, double.MaxValue)]
        public double Price { get; set; }
    }
}
