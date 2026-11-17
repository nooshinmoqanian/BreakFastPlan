using DataAccess.DBContexts;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DTO;

namespace DataAccess.Repositories
{
    public class OrderRepository : IRepositories<Order>
    {
        private readonly BreakfastContext _context;

        public OrderRepository(BreakfastContext context)
        {
            _context = context;
        }

        public async Task<Result> CreateAsync(Order entity)
        {
            await _context.Orders.AddAsync(entity);

            await _context.SaveChangesAsync();

            return new Result { Success = true, Message = "Add was successful" };
        }

        public Task<Result> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders.OrderByDescending(o => o.CreatedAt).ToListAsync();
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
                return new Order { };

            return order;
        }

        public Task<Order> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Order>> GetByUsernameAsync(string username)
        {
            return await _context.Orders
                .Where(o => o.Username == username)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<Result> UpdateAsync(Order entity)
        {
            await _context.SaveChangesAsync();

            return new Result { Success = true, Message = "Update was successful" };
        }
    }
}
