using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.DTO
{
    public class WeeklyPlanDto
    {
        public List<PlanDayDto> Days { get; set; } = new();
        public List<ShoppingItemDto> ShoppingList { get; set; } = new();
        public double TotalPrice { get; set; }
        public int AverageCalories { get; set; }
    }

    public class PlanDayDto
    {
        public int Day { get; set; }
        public string DayName { get; set; } = string.Empty;
        public PlanBreakfastDto? Breakfast { get; set; }
    }

    public class PlanBreakfastDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Calories { get; set; }
        public List<string> UnsuitableFor { get; set; } = new();
    }

    public class ShoppingItemDto
    {
        public string Name { get; set; } = string.Empty;
        public double Amount { get; set; }
        public string Unit { get; set; } = string.Empty;
        public double Price { get; set; }
    }

    public class SetPlanDayDto
    {
        public int BreakfastId { get; set; }
    }

    public class PlanOrderDto
    {
        [Required(ErrorMessage = "آدرس را وارد کنید")]
        public string Address { get; set; } = string.Empty;
    }
}
