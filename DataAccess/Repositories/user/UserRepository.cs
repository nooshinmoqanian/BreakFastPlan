using AutoMapper;
using DataAccess.DBContexts;
using DataAccess.Models;

namespace DataAccess.Repositories.user
{
    public class UserRepository : IRepository<Users>
    {
       private readonly BreackFastContext _context;
       
        public UserRepository(BreackFastContext context)
        {
           _context = context;
        }

        public async Task<bool> Add(Users entity)
        {
            if (entity != null)
             await _context.User.AddAsync(entity);

           return await _context.SaveChangesAsync() > 0;
        }
    }
}
