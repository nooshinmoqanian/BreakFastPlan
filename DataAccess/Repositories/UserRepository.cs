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
    public class UserRepository : IRepositories<Users>
    {
        private readonly BreackFastContext _context;

        public UserRepository (BreackFastContext context)
        {
            _context = context;
        }
        public async Task<Result> CreateAsync(Users entity)
        {
           var addUser = await _context.User.AddAsync(entity);
            
           var saveNewUser = await _context.SaveChangesAsync();

            if(saveNewUser > 0)
                return new Result { Success = true, Message = "Registration was successful" };

           return new Result { Success = false, Message = "Registration failed" };
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var findUser = GetByIdAsync(id);

            if (findUser != null)
            {
                _context.Remove(findUser);

                await _context.SaveChangesAsync();

                return new Result { Success = true, Message = "deleted was successful" };
            }

            return new Result { Success = false, Message = "not fount" };
        }

        public async Task<IEnumerable<Users>> GetAllAsync()
        {
            return await _context.User.ToListAsync();
        }

        public async Task<Users> GetByIdAsync(int id)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
                return new Users { };

            return user;
        }

        public async Task<Result> UpdateAsync(Users entity)
        {
            _context.User.Update(entity);

            var updateUser = await _context.SaveChangesAsync();

            if(updateUser > 0)
                return new Result { Success = true, Message = "Update was successful" };

            return new Result { Success = false, Message = "Update failed" };
        }
    }
}
