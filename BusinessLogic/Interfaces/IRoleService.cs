using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IRoleService
    {
        public string GetRoleFromToken(string token, string signKey);
    }
}
