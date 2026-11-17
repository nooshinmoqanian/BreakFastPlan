using BusinessLogic.DTO;
using BusinessLogic.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories;

namespace BusinessLogic.Services
{
    public class PlanService : IPlanService
    {
        private static readonly string[] DayNames = { "شنبه", "یکشنبه", "دوشنبه", "سه‌شنبه", "چهارشنبه", "پنجشنبه", "جمعه" };

        private readonly IRepositories<WeeklyPlanItem> _planRepository;
        private readonly IRepositories<Breakfast> _breakfastRepository;
        private readonly IRepositories<Order> _orderRepository;

        public PlanService(IRepositories<WeeklyPlanItem> planRepository, IRepositories<Breakfast> breakfastRepository, IRepositories<Order> orderRepository)
        {
            _planRepository = planRepository;
            _breakfastRepository = breakfastRepository;
            _orderRepository = orderRepository;
        }

        public async Task<WeeklyPlanDto> GetPlan(string username)
        {
            var items = (await _planRepository.GetByUsernameAsync(username)).ToList();

            var days = Enumerable.Range(0, 7).Select(day =>
            {
                var breakfast = items.FirstOrDefault(i => i.Day == day)?.Breakfast;

                return new PlanDayDto
                {
                    Day = day,
                    DayName = DayNames[day],
                    Breakfast = breakfast == null ? null : new PlanBreakfastDto
                    {
                        Id = breakfast.Id,
                        Name = breakfast.Name,
                        Calories = breakfast.Ingredients.Sum(i => i.Calories),
                        UnsuitableFor = breakfast.HealthInfos.Where(h => !h.IsSuitable).Select(h => h.Condition).ToList()
                    }
                };
            }).ToList();

            // Same ingredient on several days becomes one line: amounts and prices are added up.
            var shoppingList = items
                .SelectMany(i => i.Breakfast!.Ingredients)
                .GroupBy(i => new { i.Name, i.Unit })
                .Select(g => new ShoppingItemDto
                {
                    Name = g.Key.Name,
                    Unit = g.Key.Unit,
                    Amount = g.Sum(i => i.Amount),
                    Price = g.Sum(i => i.Price)
                })
                .OrderBy(i => i.Name)
                .ToList();

            var plannedDays = days.Where(d => d.Breakfast != null).ToList();

            return new WeeklyPlanDto
            {
                Days = days,
                ShoppingList = shoppingList,
                TotalPrice = shoppingList.Sum(i => i.Price),
                AverageCalories = plannedDays.Count == 0 ? 0 : (int)Math.Round(plannedDays.Average(d => d.Breakfast!.Calories))
            };
        }

        public async Task<bool> SetDay(string username, int day, int breakfastId)
        {
            var breakfast = await _breakfastRepository.GetByIdAsync(breakfastId);

            if (breakfast.Id == 0) return false;

            var items = await _planRepository.GetByUsernameAsync(username);

            var item = items.FirstOrDefault(i => i.Day == day);

            if (item == null)
            {
                await _planRepository.CreateAsync(new WeeklyPlanItem { Username = username, Day = day, Breakfast = breakfast });
            }
            else
            {
                item.Breakfast = breakfast;

                await _planRepository.UpdateAsync(item);
            }

            return true;
        }

        public async Task ClearDay(string username, int day)
        {
            var items = await _planRepository.GetByUsernameAsync(username);

            var item = items.FirstOrDefault(i => i.Day == day);

            if (item != null) await _planRepository.DeleteAsync(item.Id);
        }

        public async Task<Order?> OrderShoppingList(string username, string address)
        {
            var plan = await GetPlan(username);

            if (plan.ShoppingList.Count == 0) return null;

            var order = new Order
            {
                Username = username,
                OrderType = "WeeklyPlan",
                BreakfastName = "لیست خرید برنامه‌ی هفتگی",
                Quantity = 1,
                TotalPrice = plan.TotalPrice,
                Address = address
            };

            await _orderRepository.CreateAsync(order);

            return order;
        }
    }
}
