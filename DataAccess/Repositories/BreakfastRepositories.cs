using DataAccess.DBContexts;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DTO;


namespace DataAccess.Repositories
{
    public class BreakfastRepositories : IRepositories<Breakfast>
    {
        private readonly BreakfastContext _context;

        public BreakfastRepositories(BreakfastContext context)
        {
            _context = context;
        }

        public async Task<Result> CreateAsync(Breakfast entity)
        {
            var addBreakfast = await _context.Breakfasts.AddAsync(entity);

            var saveNewBreakfast = await _context.SaveChangesAsync();

            if (saveNewBreakfast > 0)
                return new Result { Success = true, Message = "Registration was successful" };

            return new Result { Success = false, Message = "Registration failed" };
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var findBreakfast = GetByIdAsync(id);

            if (findBreakfast != null)
            {
                _context.Remove(findBreakfast);

                await _context.SaveChangesAsync();

                return new Result { Success = true, Message = "deleted was successful" };
            }

            return new Result { Success = false, Message = "not fount" };
        }

        public async Task<IEnumerable<Breakfast>> GetAllAsync()
        {
            return await _context.Breakfasts.ToListAsync();
        }

        public async Task<Breakfast> GetByIdAsync(int id)
        {
            var breakfast = await _context.Breakfasts.FindAsync(id);

            if (breakfast == null)
                return new Breakfast { };

            return breakfast;
        }

        public async Task<Breakfast> GetByNameAsync(string name)
        {
            var breakfast = await _context.Breakfasts
                             .SingleOrDefaultAsync(u => u.Name == name);

            if (breakfast == null)
                return new Breakfast { };

            return breakfast;
        }

        public async Task<Result> UpdateAsync(Breakfast entity)
        {
            _context.Breakfasts.Update(entity);

            var updateUser = await _context.SaveChangesAsync();

            if (updateUser > 0)
                return new Result { Success = true, Message = "Update was successful" };

            return new Result { Success = false, Message = "Update failed" };
        }
    }
}
