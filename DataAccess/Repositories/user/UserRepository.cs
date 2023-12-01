using AutoMapper;
using DataAccess.DBContexts;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.user
{
    public class UserRepository : IRepository<Users>
    {
        private readonly BreackFastContext _context;

        public UserRepository(BreackFastContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(Users entity)
        {
            if (entity != null)
                await _context.User.AddAsync(entity);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<string> ChekeAndResultAsync(Users entity)
        {
            var userPass = await _context.User.Where(user => user.Username == entity.Username).Select(user => user.Password).FirstOrDefaultAsync();

            if (userPass != null)
                return userPass;

            return "not found";
        }

        public async Task<string> FindByNameAsync(Users entity)
        {
            var user = await _context.User.SingleOrDefaultAsync(u => u.Username == entity.Username);
            return user.Username;
        }
    }
}
