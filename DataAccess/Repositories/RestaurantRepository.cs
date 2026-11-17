using DataAccess.DBContexts;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DTO;

namespace DataAccess.Repositories
{
    public class RestaurantRepository : IRepositories<Restaurant>
    {
        private readonly BreakfastContext _context;

        public RestaurantRepository(BreakfastContext context)
        {
            _context = context;
        }

        public async Task<Result> CreateAsync(Restaurant entity)
        {
            await _context.Restaurants.AddAsync(entity);

            await _context.SaveChangesAsync();

            return new Result { Success = true, Message = "Add was successful" };
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var findRestaurant = await GetByIdAsync(id);

            if (findRestaurant.Id != 0)
            {
                _context.Remove(findRestaurant);

                await _context.SaveChangesAsync();

                return new Result { Success = true, Message = "deleted was successful" };
            }

            return new Result { Success = false, Message = "not fount" };
        }

        public async Task<IEnumerable<Restaurant>> GetAllAsync()
        {
            return await WithMenu().ToListAsync();
        }

        public async Task<Restaurant> GetByIdAsync(int id)
        {
            var restaurant = await WithMenu().SingleOrDefaultAsync(r => r.Id == id);

            if (restaurant == null)
                return new Restaurant { };

            return restaurant;
        }

        public async Task<Restaurant> GetByNameAsync(string name)
        {
            var restaurant = await WithMenu().FirstOrDefaultAsync(r => r.Name == name);

            if (restaurant == null)
                return new Restaurant { };

            return restaurant;
        }

        public async Task<Result> UpdateAsync(Restaurant entity)
        {
            await _context.SaveChangesAsync();

            return new Result { Success = true, Message = "Update was successful" };
        }

        private IQueryable<Restaurant> WithMenu()
        {
            return _context.Restaurants
                .Include(r => r.RestaurantBreakfasts).ThenInclude(rb => rb.Breakfast);
        }
    }
}
