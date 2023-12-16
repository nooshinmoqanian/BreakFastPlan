using DataAccess.DBContexts;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class BreakfastTagRepository : IRepositories<Tag>
    {
        private readonly BreakfastContext _context;

        public BreakfastTagRepository(BreakfastContext context)
        {
            _context = context;
        }

        public async Task<Result> CreateAsync(Tag entity)
        {
            var addBreakfast = await _context.Tags.AddAsync(entity);

            var saveNewBreakfast = await _context.SaveChangesAsync();

            if (saveNewBreakfast > 0)
                return new Result { Success = true, Message = "Add was successful" };

            return new Result { Success = false, Message = "this failed" };
        }

        public Task<Result> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Tag>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Tag> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Tag> GetByNameAsync(string name)
        {
            var findTagName =  await _context.Tags.SingleOrDefaultAsync(u => u.Name == name);

            if (findTagName != null)
                return findTagName;

            return new Tag { };                 
        }

        public Task<Result> UpdateAsync(Tag entity)
        {
            throw new NotImplementedException();
        }
    }
}
