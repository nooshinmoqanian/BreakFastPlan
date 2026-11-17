using BusinessLogic.DTO;
using DataAccess.Models;

namespace BusinessLogic.Interfaces
{
    public interface IOrderService
    {
        Task<Order?> CreateOrder(string username, CreateOrderDto order);
        Task<IEnumerable<Order>> GetMyOrders(string username);
        Task<IEnumerable<Order>> GetAllOrders();
        Task<bool> UpdateStatus(int id, string status);
    }
}
