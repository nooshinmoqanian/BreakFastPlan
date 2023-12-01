using DataAccess.DBContexts;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.token
{
    public class TokenRepository
    {
        private readonly BreackFastContext _context;

        public TokenRepository(BreackFastContext context)
        {
            _context = context;
        }

        public async Task<string> GetToken(Users entity)
        {
            var refreshToken = await _context.User.Where(u=> u.Username == entity.Username).Select(t=> t.Token).FirstOrDefaultAsync();
            return refreshToken;
        }
    }
}
