using DataAccess.DBContexts;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DTO;

namespace DataAccess.Repositories
{
    public class PlanRepository : IRepositories<WeeklyPlanItem>
    {
        private readonly BreakfastContext _context;

        public PlanRepository(BreakfastContext context)
        {
            _context = context;
        }

        public async Task<Result> CreateAsync(WeeklyPlanItem entity)
        {
            await _context.WeeklyPlanItems.AddAsync(entity);

            await _context.SaveChangesAsync();

            return new Result { Success = true, Message = "Add was successful" };
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var item = await _context.WeeklyPlanItems.FindAsync(id);

            if (item == null) return new Result { Success = false, Message = "not fount" };

            _context.Remove(item);

            await _context.SaveChangesAsync();

            return new Result { Success = true, Message = "deleted was successful" };
        }

        public Task<IEnumerable<WeeklyPlanItem>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<WeeklyPlanItem> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<WeeklyPlanItem> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<WeeklyPlanItem>> GetByUsernameAsync(string username)
        {
            return await _context.WeeklyPlanItems
                .Where(p => p.Username == username)
                .Include(p => p.Breakfast).ThenInclude(b => b!.Ingredients)
                .Include(p => p.Breakfast).ThenInclude(b => b!.HealthInfos)
                .AsSplitQuery()
                .ToListAsync();
        }

        public async Task<Result> UpdateAsync(WeeklyPlanItem entity)
        {
            await _context.SaveChangesAsync();

            return new Result { Success = true, Message = "Update was successful" };
        }
    }
}
