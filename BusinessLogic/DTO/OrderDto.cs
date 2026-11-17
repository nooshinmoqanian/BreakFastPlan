using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.DTO
{
    public class CreateOrderDto
    {
        public int BreakfastId { get; set; }
        [RegularExpression("Ingredients|Restaurant")]
        public string OrderType { get; set; } = "Ingredients";
        public int? RestaurantId { get; set; }
        [Range(1, 20)]
        public int Quantity { get; set; } = 1;
        [Required]
        public string Address { get; set; } = string.Empty;
    }

    public class UpdateOrderStatusDto
    {
        [RegularExpression("Pending|Preparing|Delivered|Canceled")]
        public string Status { get; set; } = string.Empty;
    }
}
