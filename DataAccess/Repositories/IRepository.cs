using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<bool> AddAsync(T entity);
        Task<string> FindByNameAsync(T entity);
        Task<string> ChekeAndResultAsync(T entity);
    }
}
