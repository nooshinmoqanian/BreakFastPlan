using BusinessLogic.DTO;
using BusinessLogic.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories;

namespace BusinessLogic.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepositories<Order> _orderRepository;
        private readonly IRepositories<Breakfast> _breakfastRepository;

        public OrderService(IRepositories<Order> repositories, IRepositories<Breakfast> breakfastRepository)
        {
            _orderRepository = repositories;
            _breakfastRepository = breakfastRepository;
        }

        public async Task<Order?> CreateOrder(string username, CreateOrderDto order)
        {
            var breakfast = await _breakfastRepository.GetByIdAsync(order.BreakfastId);

            if (breakfast.Id == 0) return null;

            var newOrder = new Order
            {
                Username = username,
                OrderType = order.OrderType,
                BreakfastId = breakfast.Id,
                BreakfastName = breakfast.Name,
                Quantity = order.Quantity,
                Address = order.Address
            };

            // The price always comes from the database, never from the client.
            if (order.OrderType == "Restaurant")
            {
                var menuItem = breakfast.RestaurantBreakfasts.FirstOrDefault(rb => rb.RestaurantId == order.RestaurantId);

                if (menuItem == null) return null;

                newOrder.RestaurantId = menuItem.RestaurantId;
                newOrder.RestaurantName = menuItem.Restaurant!.Name;
                newOrder.TotalPrice = menuItem.Price * order.Quantity;
            }
            else
            {
                newOrder.TotalPrice = breakfast.Ingredients.Sum(i => i.Price) * order.Quantity;
            }

            await _orderRepository.CreateAsync(newOrder);

            return newOrder;
        }

        public Task<IEnumerable<Order>> GetMyOrders(string username)
        {
            return _orderRepository.GetByUsernameAsync(username);
        }

        public Task<IEnumerable<Order>> GetAllOrders()
        {
            return _orderRepository.GetAllAsync();
        }

        public async Task<bool> UpdateStatus(int id, string status)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order.Id == 0) return false;

            order.Status = status;

            var updateResult = await _orderRepository.UpdateAsync(order);

            return updateResult.Success;
        }
    }
}
