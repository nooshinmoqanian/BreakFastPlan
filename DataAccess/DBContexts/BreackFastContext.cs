using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DBContexts
{
    public class BreackFastContext : DbContext
    {
        public BreackFastContext(DbContextOptions<BreackFastContext> options)
            : base(options)
        {
        }

        public DbSet<Users> User { get; set; } = default!;
    }

}
