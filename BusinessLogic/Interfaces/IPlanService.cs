using BusinessLogic.DTO;
using DataAccess.Models;

namespace BusinessLogic.Interfaces
{
    public interface IPlanService
    {
        Task<WeeklyPlanDto> GetPlan(string username);
        Task<bool> SetDay(string username, int day, int breakfastId);
        Task ClearDay(string username, int day);
        Task<Order?> OrderShoppingList(string username, string address);
    }
}
