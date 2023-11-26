using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.user
{
    public interface IRepository<T> where T : class
    {
         Task<bool> Add(T entity);
    }
}
